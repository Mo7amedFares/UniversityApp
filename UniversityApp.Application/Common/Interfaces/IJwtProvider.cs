using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Common.Interfaces
{
    public interface IJwtProvider
    {
        public string Generate(User user);
    }
}
