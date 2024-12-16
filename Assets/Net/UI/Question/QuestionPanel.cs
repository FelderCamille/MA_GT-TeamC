using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using BaseUI = UI;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to manage the 'store' panel
	/// </summary>
	public class QuestionPanel : PanelInteractive
	{
		[Header("Place where the question is printed")]
		public TMPro.TextMeshProUGUI QuestionTitle;

		[Header("Place where the questions will be instanciated")]
		public LayoutGroup QuestionsSpace;

		[Header("Content")]
		public BaseUI.QuestionButton ButtonPrefab;

		// TODO: get from the mine?
		private Question[] questions;

		private readonly List<BaseUI.QuestionButton> buttons = new();

		public override bool IsOpen()
		{
			return this.gameObject.activeSelf;
		}

		public override void Close()
		{
			this.cleanContent();
			this.gameObject.SetActive(false);
		}

		public void Open(PlayerController player, MineController mine)
		{
			var question = this.getQuestion(mine);
			this.updateContent(player, mine, question);

			this.gameObject.SetActive(true);
		}

		/// <summary>
		/// Toggle this panel
		/// </summary>
		/// <param name="player">That opens the panel</param>
		/// <param name="mine">for the panel (with question)</param>
		public void Toogle(PlayerController player, MineController mine)
		{
			if (this.IsOpen())
			{
				this.Close();
			}
			else
			{
				this.Open(player, mine);
			}
		}

		// Show the question and the possible responses (also add listeners for the buttons)
		private void updateContent(PlayerController player, MineController mine, Question question)
		{
			// clean before updating
			this.cleanContent();

			// THE question itself
			this.QuestionTitle.text = question.query;

			// `-1` because the index start at 1 ...
			var correctIndex = question.correctIndex - 1;
			for (var i = 0; i < question.responses.Length; ++i)
			{
				var response = question.responses[i];

				var button = Instantiate(
					this.ButtonPrefab,
					Vector3.zero,
					Quaternion.identity,
					this.QuestionsSpace.transform
				);
				this.buttons.Add(button);

				button.Init(
					response,
					this.HandleResponseClicked(player, mine, button, correctIndex, i)
				);
			}
		}

		private void cleanContent()
		{
			foreach (var button in buttons)
			{
				DestroyImmediate(button.gameObject);
			}

			buttons.Clear();
		}

		private Question getQuestion(MineController mine)
		{
			if (this.questions == null)
			{
				this.questions = JsonUtils<Questions>.Read("Json/questions").Shuffle();
			}

			// TODO: from mine
			return this.questions[new System.Random().Next(0, this.questions.Length)];
		}

		/// <param name="player">Player that answered</param>
		/// /// <param name="mine">The mine on which the player answered</param>
		/// <param name="questionIndex">The "position" of the given button/question</param>
		private IEnumerator HandleResponseClicked(
			PlayerController player,
			MineController mine,
			BaseUI.QuestionButton button,
			int correctIndex,
			int questionIndex
		)
		{
			this.SoundManager.PlayCutSound();

			// if incorrect, also show the correct answer
			var isCorrect = correctIndex == questionIndex;
			if (!isCorrect)
			{
				StartCoroutine(buttons[correctIndex].ShowResult(true));
			}

			yield return StartCoroutine(button.ShowResult(isCorrect));

			// Update game through player
			player.DemineResult(mine, isCorrect);
			this.Close();
		}
	}
}
