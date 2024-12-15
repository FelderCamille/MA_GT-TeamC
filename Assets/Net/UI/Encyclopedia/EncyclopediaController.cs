using Core;
using Objects;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Net
{
	// Mosltly taken from the other EncyclopediaController
	public class EncyclopediaController : MonoBehaviour
	{
		[Header("The sound manager")]
		public SoundManager SoundManager;

		[Header("Content")]
		public SectionTab sectionTabPrefab;
		public NavigationButton previousPageButton;
		public NavigationButton nextPageButton;
		public Text pageNumber;

		// Navigation
		private Section[] _sections;
		private int _currentSectionIndex = 0;
		private int _currentPageIndex = 0;

		void Awake()
		{
			// Load encyclopedia
			_sections = JsonUtils<Encyclopedia>.Read("Json/encyclopedia").sections;

			// Construct sections tabs
			for (var i = 0; i < _sections.Length; i++)
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

				var section = _sections[i];
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
			var currentSection = _sections[_currentSectionIndex];
			if (sectionTab.Text == currentSection.title)
			{
				_currentPageIndex = 0;
				UpdatePageContent();
				return;
			}
			// If other section, change section
			SetSection(sectionTab.Index);
		}

		private void SetSection(int index)
		{
			// Update section
			_currentSectionIndex = index;
			_currentPageIndex = 0; // Reset page index
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
			++_currentPageIndex;
			UpdatePageContent();
		}

		private void HandlePreviousPageClick()
		{
			if (!HasPreviousPage())
			{
				return;
			}

			// Go to previous page
			--_currentPageIndex;
			UpdatePageContent();
		}

		private void UpdatePageContent()
		{
			// Update page content
			var currentSection = _sections[_currentSectionIndex];
			var currentPage = currentSection.pages[_currentPageIndex];

			GetComponentInChildren<TextMeshProUGUI>().text =
				"<style=\"Title\">" + currentPage.title + "</style>\n\n" + currentPage.content;
			// Update page number
			pageNumber.text = (_currentPageIndex + 1) + "/" + currentSection.pages.Length;

			// Update previous button state
			this.previousPageButton.SetEnable(this.HasPreviousPage());
			this.nextPageButton.SetEnable(this.HasNextPage());
		}

		private bool HasPreviousPage() => _currentPageIndex > 0;

		private bool HasNextPage() =>
			_currentPageIndex < _sections[_currentSectionIndex].pages.Length - 1;
	}
}
