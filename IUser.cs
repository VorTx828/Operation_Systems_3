﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPract3
{
    public interface IUser
    {
        void Send(string message);
        void Quit();
    }
}
