using Objects;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controllers
{
    public class EncyclopediaController : MonoBehaviour
    {

        [Header("Content")]
        public SectionTab sectionTabPrefab;
        public NavigationButton previousPageButton;
        public NavigationButton nextPageButton;

        private Section[] _sections;
        private int _currentSectionIndex = 0;
        private int _currentPageIndex = 0;

        private bool _isOpened;

        public bool IsOpened => _isOpened;

        private void Awake()
        {
            // Load encyclopedia
            var encyclopediaObj = JsonUtils<Encyclopedia>.Read("Json/encyclopedia");
            _sections = encyclopediaObj.sections;
            // Construct sections tabs
            for (var i = 0; i < _sections.Length; i++)
            {
                var sectionTabObj = Instantiate(sectionTabPrefab, Vector3.zero, Quaternion.identity);
                sectionTabObj.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform, false); // To avoid the Transform component to be at (0,0,0)
                var section = _sections[i];
                sectionTabObj.name = "Section tab n°" + i + " (\"" + section.title + "\")";
                sectionTabObj.Init(section.title, i, OnSectionClicked);
            }
            // Set previous and next page buttons listeners
            previousPageButton.Init(false, PreviousPage);
            nextPageButton.Init(true, NextPage);
        }

        private void OnEnable()
        {
            // Change is opened status
            _isOpened = true;
            // Display encyclopedia page
            UpdatePageContent();
        }

        private void OnDisable()
        {
            _isOpened = false;
        }

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

        private void UpdatePageContent()
        {
            // Update page content
            var currentSection = _sections[_currentSectionIndex];
            var currentPage = currentSection.pages[_currentPageIndex];
            GetComponentInChildren<TextMeshProUGUI>().text = "<style=\"Title\">" + currentPage.title + "</style>\n\n" + currentPage.content;
            // Update previous button state
            if (HasPreviousPage() && !previousPageButton.IsEnabled)
            {
                previousPageButton.Enable();
            } else if (!HasPreviousPage() && previousPageButton.IsEnabled)
            {
                previousPageButton.Disable();
            }
            // Update next button state
            if (HasNextPage() && !nextPageButton.IsEnabled)
            {
                nextPageButton.Enable();
            } else if (!HasNextPage() && nextPageButton.IsEnabled)
            {
                nextPageButton.Disable();
            }
        }
        
        private void NextPage()
        {
            // If it can't go to next page, return
            if (!HasNextPage()) return;
            // Go to next page
            _currentPageIndex++;
            UpdatePageContent();
        }

        private void PreviousPage()
        {
            // It it can't go to previous page, return
            if (!HasPreviousPage()) return;
            // Go to previous page
            _currentPageIndex--;
            UpdatePageContent();
        }

        private bool HasPreviousPage() => _currentPageIndex > 0;
        private bool HasNextPage() => _currentPageIndex < _sections[_currentSectionIndex].pages.Length - 1;

    }
}
