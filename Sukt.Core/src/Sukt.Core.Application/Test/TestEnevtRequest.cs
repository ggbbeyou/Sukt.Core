﻿using Sukt.Core.Dtos.Menu;
using Sukt.Core.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sukt.Core.Application.Test
{
    public class TestEnevtRequest: RequestEventBase<MenuInputDto>
    {

        public string Test { get; set; }
    }
}
