using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerController : NetworkBehaviour {
    // TODO: from the GameController
    [Header("Mine prefab")]
    public GameObject mine;

    // "In-game" camera
    public Camera MyCamera;

    // Arrow that inidcates that the player is looking at an interactive lenght of a mine
    public GameObject lookingAtAMineInd;

    // The body of the robot (with collider and some physic)
    public Rigidbody robot;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        // TODO: Register to a game controller that should/will
        //  get the playground and pose correclty the player
        //  identify it as player 1 or player 2
        //  Sync to start the game
        //  Kinda the library to known the use elements (mines, questions, ...)

        robot.position = new Vector3(-100, 1, 0);
        // Debug.Log($"a b c c server: {this.IsServer} - client {this.IsClient} - local {this.IsLocalPlayer} - ");

        // For the "in-game" camera
        //this.MyCamera.gameObject.SetActive(this.IsLocalPlayer);
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
        // TODO
    }

    // Variables for network movement.
    // TODO: a client rigibody transform for that ?
    private bool movingUp = false;
    private bool movingDown = false;
    private bool rotatingL = false;
    private bool rotatingR = false;

    void FixedUpdate() {
        if (!this.IsServer) {
            // Only the server side does the "real" updates.
            return;
        }

        var rb = robot;
        var movDelta = 7 * Time.fixedDeltaTime;
        if (this.movingUp) {
            rb.MovePosition(rb.position + rb.rotation * Vector3.forward * movDelta);
        }
        if (this.movingDown) {
            rb.MovePosition(rb.position - rb.rotation * Vector3.forward * movDelta);
        }

        var rotateDelta = Time.fixedDeltaTime * 250;
        if (this.rotatingL) {
           var r = rb.rotation.eulerAngles + new Vector3(0, -1, 0) * rotateDelta;
           rb.MoveRotation(Quaternion.Euler(r.x, r.y, r.z));
        }
        if (this.rotatingR) {
           var r = rb.rotation.eulerAngles + new Vector3(0, 1, 0) * rotateDelta;
           rb.MoveRotation(Quaternion.Euler(r.x, r.y, r.z));
        }
    }

    [Rpc(SendTo.Server)]
    void moveUpRpc(bool up) {
        this.movingUp = up;
    }
    [Rpc(SendTo.Server)]
    void moveDownRpc(bool down) {
        this.movingDown = down;
    }
    [Rpc(SendTo.Server)]
    void rotateLRpc(bool down) {
        this.rotatingL = down;
    }
    [Rpc(SendTo.Server)]
    void rotateRRpc(bool down) {
        this.rotatingR = down;
    }
    [Rpc(SendTo.Server)]
    void MineRpc() {
        if (this.editableMines.Count > 0) {
            Debug.Log("CAN not set a mine near another one");
            // TODO: better (check with collider)
            return;
        }

        // TODO: check (and use a Game manager)
        this.SetMineRpc(this.robot.position, this.robot.rotation);
    }

    [Rpc(SendTo.Everyone)]
    void SetMineRpc(Vector3 pos, Quaternion rot) {
        var mine = Instantiate(this.mine, pos, rot);

        var c = mine.GetComponent<MineController>();
        c.firstInit = true;
    }


    void Update() {
        if (!this.IsLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(Constants.Actions.MoveUp)) {
            this.moveUpRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveUp)) {
            this.moveUpRpc(false);
        }

        if (Input.GetKeyDown(Constants.Actions.MoveDown)) {
            this.moveDownRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveDown)) {
            this.moveDownRpc(false);
        }


        if (Input.GetKeyDown(Constants.Actions.MoveLeft)) {
            this.rotateLRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveLeft)) {
            this.rotateLRpc(false);
        }


        if (Input.GetKeyDown(Constants.Actions.MoveRight)) {
            this.rotateRRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveRight)) {
            this.rotateRRpc(false);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.MineRpc();
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            this.hasVision = !this.hasVision;
        }

        // TODO: only temp
        List<MineController> toRemove = new ();
        foreach (var mine in this.editableMines) {
            if (!mine.gameObject.activeSelf) {
                toRemove.Add(mine);
            }
        }
        foreach (var t in toRemove) {
            this.editableMines.Remove(t);
        }

        if (Physics.Raycast(robot.position, robot.rotation * Vector3.forward, out var hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.TryGetComponent<MineController>(out var seeingMine)) {
                foreach (var mineX in this.editableMines) {
                    if (seeingMine == mineX) {
                        if (this.currentlyEditable != null) {
                            if (this.currentlyEditable == seeingMine) {
                                // Same mine
                                return;
                            }

                            this.currentlyEditable.UnshowAsUsabe();
                        }


                        this.lookingAtAMineInd.SetActive(true);
                        this.currentlyEditable = seeingMine;
                        seeingMine.ShowAsUsabe();
                        return;
                    }
                }
            }
        }

        if (this.currentlyEditable != null) {
            this.currentlyEditable.UnshowAsUsabe();
            this.currentlyEditable = null;
        }

        this.lookingAtAMineInd.SetActive(false);
    }

    private MineController currentlyEditable = null;

    // Bonus
    private bool hasVision = false;
    // Mine that could be demined // TODO: this is probably not optimum
    private List<MineController> editableMines = new ();

    public void SeeMine(MineController mine, PlayerCollider.TYPE cType) {
        if (!this.IsLocalPlayer) {
            // Ignore other player
            return;
        }

        if (cType == PlayerCollider.TYPE.NORMAL) {
            this.editableMines.Add(mine);
        }

        if (cType == PlayerCollider.TYPE.EXTENDED && !this.hasVision) {
            return;
        }

        mine.MarkAsShown();
    }

    public void SeeNoMine(MineController mine, PlayerCollider.TYPE cType) {
        if (!this.IsLocalPlayer) {
            // Ignore other player
            return;
        }

        if (cType == PlayerCollider.TYPE.NORMAL) {
            this.editableMines.Remove(mine);
        }

        if (cType == PlayerCollider.TYPE.NORMAL && this.hasVision) {
            return;
        }

        mine.MarkAsUnshown();
    }
}
