﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheCodeCamp.Data;

namespace TheCodeCamp.Model
{
    public class TalkModel
    {
        public int TalkId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }
        public SpeakerModel Speaker { get; set; }
    }
}