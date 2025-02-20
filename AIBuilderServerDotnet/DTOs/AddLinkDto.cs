﻿using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class AddLinkDto
    {
        [Required]
        public int Position { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string PageName { get; set; }
        [Required]
        public string ModalType { get; set; }
        [Required]
        public int WidgetPosition { get; set; }
        [Required]
        public int ModalPosition { get; set; }
    }
}
