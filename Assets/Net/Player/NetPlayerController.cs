using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerController : NetworkBehaviour {


    [Header("Mine prefab")]
    public GameObject mine;

    public Camera MyCamera;

    public GameObject lookINgArrow;

    public Rigidbody robot;

    public override void OnNetworkSpawn() {
        base.OnNetworkDespawn();

        // TODO
        robot.position = new Vector3(-100, 1, 0);

        Debug.Log($"a b c c server: {this.IsServer} - client {this.IsClient} - local {this.IsLocalPlayer} - ");

        //this.MyCamera.gameObject.SetActive(this.IsLocalPlayer);
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
    }

    [Rpc(SendTo.Server)]
    void MoveUpRpc(Vector3 pos) {
        Debug.Log("UP");
        var rb = robot;
        rb.MovePosition(pos);
    }

    private bool movUp = false;
    private bool movDow = false;
    private bool rotL = false;
    private bool rotR = false;

    void FixedUpdate() {
        if (!this.IsServer) {
            // Only the server side does the "real" updates.
            return;
        }

        var rb = robot;
        var movDelta = 7 * Time.fixedDeltaTime;
        if (this.movUp) {
            rb.MovePosition(rb.position + rb.rotation * Vector3.forward * movDelta);
        }
        if (this.movDow) {
            rb.MovePosition(rb.position - rb.rotation * Vector3.forward * movDelta);
        }

        var rotateDelta = Time.fixedDeltaTime * 250;
        if (this.rotL) {
           var r = rb.rotation.eulerAngles + new Vector3(0, -1, 0) * rotateDelta;
           rb.MoveRotation(Quaternion.Euler(r.x, r.y, r.z));
        }
        if (this.rotR) {
           var r = rb.rotation.eulerAngles + new Vector3(0, 1, 0) * rotateDelta;
           rb.MoveRotation(Quaternion.Euler(r.x, r.y, r.z));
        }
    }

    [Rpc(SendTo.Server)]
    void moveUpRpc(bool up) {
        this.movUp = up;
    }

    [Rpc(SendTo.Server)]
    void moveDownRpc(bool down) {
        this.movDow = down;
    }

    [Rpc(SendTo.Server)]
    void rotLRpc(bool down) {
        this.rotL = down;
    }

    [Rpc(SendTo.Server)]
    void rotRRpc(bool down) {
        this.rotR = down;
    }

    [Rpc(SendTo.Server)]
    void MineRpc() {
        // TODO: check (and use a Game manager)
        this.SetMineRpc(this.robot.position, this.robot.rotation);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void SetMineRpc(Vector3 pos, Quaternion rot) {
        var mine = Instantiate(this.mine, pos, rot);
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
            this.rotLRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveLeft)) {
            this.rotLRpc(false);
        }


        if (Input.GetKeyDown(Constants.Actions.MoveRight)) {
            this.rotRRpc(true);
        }
        if (Input.GetKeyUp(Constants.Actions.MoveRight)) {
            this.rotRRpc(false);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.MineRpc();
        }


        if (Input.GetKeyDown(KeyCode.V)) {
            this.hasVision = !this.hasVision;
            Debug.Log($"CISON {this.hasVision}");
        }

        //Debug.Log($"seem {this.seenMines.Count}");

        if (Physics.Raycast(robot.position, robot.rotation * Vector3.forward, out var hit, Mathf.Infinity)) {
            //Debug.Log($"looking at {hit.collider.gameObject.name}");
            if (hit.collider.gameObject.TryGetComponent<MineController>(out var seeingMine)) {
                foreach (var mineX in this.seenMines) {
                    if (seeingMine == mineX) {
                        if (this.currentlySeeing != null) {
                            if (this.currentlySeeing == seeingMine) {
                                // Same mine
                                return;
                            }

                            this.currentlySeeing.UnshowAsUsabe();                  
                        }


                        this.lookINgArrow.SetActive(true);
                        this.currentlySeeing = seeingMine;
                        seeingMine.ShowAsUsabe();
                        return;
                    }
                }
            }
        }

        if (this.currentlySeeing != null) {
            this.currentlySeeing.UnshowAsUsabe();
            this.currentlySeeing = null;
        }

        this.lookINgArrow.SetActive(false);
    }

    private MineController currentlySeeing = null;

    private bool hasVision = false;
    private List<MineController> seenMines = new ();

    public void SeeMine(MineController mine, PlayerCollider.TYPE cType) {
        if (!this.IsLocalPlayer) {
            return;
        }

        if (cType == PlayerCollider.TYPE.NORMAL) {
            this.seenMines.Add(mine);
        }

        if (cType == PlayerCollider.TYPE.EXTENDED && !this.hasVision) {
            return;
        }

        mine.MarkAsShown();
    }

    public void SeeNoMine(MineController mine, PlayerCollider.TYPE cType) {
        if (!this.IsLocalPlayer) {
            return;
        }

        if (cType == PlayerCollider.TYPE.NORMAL) {
            this.seenMines.Remove(mine);
        }

        if (cType == PlayerCollider.TYPE.NORMAL && this.hasVision) {
            return;
        }

        mine.MarkAsUnshown();
    }
}
