﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class ChanAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.ChanAftertouch; }
        }
    }
}
