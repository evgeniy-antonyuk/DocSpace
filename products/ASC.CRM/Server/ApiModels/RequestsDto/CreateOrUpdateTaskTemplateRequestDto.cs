﻿using System;

namespace ASC.CRM.ApiModels
{
    public class CreateOrUpdateTaskTemplateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Responsibleid { get; set; }
        public int Categoryid { get; set; }
        public bool isNotify { get; set; }
        public long OffsetTicks { get; set; }
        public bool DeadLineIsFixed { get; set; }
    }
}
