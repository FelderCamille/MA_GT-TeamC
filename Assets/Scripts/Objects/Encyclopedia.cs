using System;

namespace Objects
{
    [Serializable]
    public class Encyclopedia
    {
        /// <summary>
        ///  Sections of the encyclopedia
        /// </summary>
        public Section[] sections;
    }
    
    [Serializable]
    public class Section
    {
        /// <summary>
        /// Title of section
        /// </summary>
        public string title;
        
        /// <summary>
        /// Pages in the section
        /// </summary>
        public Page[] pages;
    }
    
    [Serializable]
    public class Page
    {
        /// <summary>
        /// Title of the page
        /// </summary>
        public string title;
        
        /// <summary>
        /// Content of the page
        /// </summary>
        public string content;
    }
}