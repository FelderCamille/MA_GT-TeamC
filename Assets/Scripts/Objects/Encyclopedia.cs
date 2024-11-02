using System;

namespace Objects
{
    [Serializable]
    public class Encyclopedia
    {
        public Section[] sections;
    }
    
    [Serializable]
    public class Section
    {
        public string title;
        public Page[] pages;
    }
    
    [Serializable]
    public class Page
    {
        public string title;
        public string content;
    }
}