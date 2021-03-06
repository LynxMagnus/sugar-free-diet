﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SugarFreeDiet.Models
{
    [Table("Recipes")]
    public class Recipe
    {
        public int RecipeId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMMM yyyy}")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Short Description is required")]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
                
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ingredients are required")]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }

        [Required(ErrorMessage = "Directions are required")]
        [DataType(DataType.MultilineText)]
        public string Directions { get; set; }

        [Required(ErrorMessage = "Hours are required")]
        [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
        public int Hours { get; set; }

        [Required(ErrorMessage = "Minutes are required")]
        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59")]
        public int Minutes { get; set; }

        [Required(ErrorMessage = "Servings are required")]
        [Range(1, 50, ErrorMessage = "Servings must be between 1 and 50")]
        public int Servings { get; set; }

        [Display(Name ="Suitable for Vegetarians")]
        public bool Vegetarian { get; set; }

        [Display(Name = "Suitable for Vegans")]
        public bool Vegan { get; set; }
        
        public byte[] Image { get; set; }

        public byte[] Thumbnail { get; set; }

        public bool Active { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public string Duration
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (Hours >= 1)
                {
                    sb.Append(string.Format("{0} ", Hours));
                    if (Hours == 1)
                    {
                        sb.Append("hour ");
                    }
                    else
                    {
                        sb.Append("hours ");
                    }
                }

                if (Minutes >= 1)
                {
                    sb.Append(string.Format("{0} ", Minutes));
                    if (Minutes == 1)
                    {
                        sb.Append("minute");
                    }
                    else
                    {
                        sb.Append("minutes");
                    }
                }

                return sb.ToString();
            }
        }

        public void SetCreated(string userId)
        {
            UserId = userId;
            Created = DateTime.Now;
        }

        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", RecipeId, Title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}