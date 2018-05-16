using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{
    public class MessageViewModel
    {
        public MessageViewModel() { }
        public MessageViewModel(string title, string description, string currentpage, MessageTypes type)
        {
            Title = title;
            Description = description;
            CurrentPage = currentpage;
            Type = type;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentPage { get; set; }
        public MessageTypes Type { get; set; }
        public enum MessageTypes
        {
            Success,
            Info,
            Warning,
            Error
        }
    }
}