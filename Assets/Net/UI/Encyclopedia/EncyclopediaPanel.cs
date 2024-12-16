using Core;
using Objects;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Net.UI
{
	// Mosltly taken from the other EncyclopediaController
	public class EncyclopediaPanel : PanelInteractive
	{
		[Header("Content")]
		public SectionTab sectionTabPrefab;
		public NavigationButton previousPageButton;
		public NavigationButton nextPageButton;
		public Text pageNumber;

		// Navigation
		private Section[] sections;
		private int currentSectionIndex = 0;
		private int currentPageIndex = 0;

		public override bool IsOpen()
		{
			return this.gameObject.activeSelf;
		}

		public void Open()
		{
			this.gameObject.SetActive(true);
		}

		public override void Close()
		{
			this.gameObject.SetActive(false);
		}

		/// <summary>
		/// Simply toggles the state of the panel
		/// </summary>
		public void Toogle()
		{
			if (this.IsOpen())
			{
				this.Close();
			}
			else
			{
				this.Open();
			}
		}

		void Awake()
		{
			// Load encyclopedia
			sections = JsonUtils<Encyclopedia>.Read("Json/encyclopedia").sections;

			// Construct sections tabs
			for (var i = 0; i < sections.Length; i++)
			{
				var sectionTabObj = Instantiate(
					sectionTabPrefab,
					Vector3.zero,
					Quaternion.identity
				);
				sectionTabObj.transform.SetParent(
					GetComponentInChildren<VerticalLayoutGroup>().transform,
					false
				); // To avoid the Transform component to be at (0,0,0)

				var section = sections[i];
				sectionTabObj.name = "Section tab n°" + i + " (\"" + section.title + "\")";
				sectionTabObj.Init(section.title, i, OnSectionClicked);
			}

			// Set previous and next page buttons listeners
			previousPageButton.Init(false, HandlePreviousPageClick);
			nextPageButton.Init(true, HandleNextPageClick);
		}

		void Start() => this.UpdatePageContent();

		private void OnEnable() => SoundManager.playOpenBookSound();

		private void OnDisable() => SoundManager.playCloseBookSound();

		private void OnSectionClicked(SectionTab sectionTab)
		{
			// If same section, reset page
			var currentSection = sections[currentSectionIndex];
			if (sectionTab.Text == currentSection.title)
			{
				currentPageIndex = 0;
				UpdatePageContent();
				return;
			}
			// If other section, change section
			SetSection(sectionTab.Index);
		}

		private void SetSection(int index)
		{
			// Update section
			currentSectionIndex = index;
			currentPageIndex = 0; // Reset page index
			// Update page
			UpdatePageContent();
		}

		private void HandleNextPageClick()
		{
			if (!HasNextPage())
			{
				return;
			}

			// Go to next page
			++currentPageIndex;
			UpdatePageContent();
		}

		private void HandlePreviousPageClick()
		{
			if (!HasPreviousPage())
			{
				return;
			}

			// Go to previous page
			--currentPageIndex;
			UpdatePageContent();
		}

		private void UpdatePageContent()
		{
			// Update page content
			var currentSection = sections[currentSectionIndex];
			var currentPage = currentSection.pages[currentPageIndex];

			GetComponentInChildren<TextMeshProUGUI>().text =
				"<style=\"Title\">" + currentPage.title + "</style>\n\n" + currentPage.content;
			// Update page number
			pageNumber.text = (currentPageIndex + 1) + "/" + currentSection.pages.Length;

			// Update previous button state
			this.previousPageButton.SetEnable(this.HasPreviousPage());
			this.nextPageButton.SetEnable(this.HasNextPage());
		}

		private bool HasPreviousPage() => currentPageIndex > 0;

		private bool HasNextPage() =>
			currentPageIndex < sections[currentSectionIndex].pages.Length - 1;
	}
}
