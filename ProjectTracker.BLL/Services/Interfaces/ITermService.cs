﻿using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface ITermService
    {
        List<Term> GetTerms();
        void ImportLessonsAsEventsFromExcel(string path, Term term);
    }
}
