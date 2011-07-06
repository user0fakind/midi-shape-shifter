﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Responsible for listening to dry MssEvents coming through the DryMssEventRelay, ensuring that they get 
    ///     processed and passing them out to the WetMssEventRelay.
    /// </summary>
    public class DryMssEventHandler
    {
        /// <summary>
        ///     Used to process incomming MssEvents
        /// </summary>
        protected MssMsgProcessor mssMsgProcessor;

        /// <summary>
        ///     Receives MssEvents once they have been processed
        /// </summary>
        protected IWetMssEventReceiver wetMssEventReceiver;

        public DryMssEventHandler()
        {
            mssMsgProcessor = new MssMsgProcessor();
        }

        /// <summary>
        ///     Initializes this DryMssEventHandler. Other methods in this class should not be called beofre calling 
        ///     Init().
        /// </summary>
        /// <param name="dryMssEventEchoer">Sends unprocessed MssEvents from the host.</param>
        /// <param name="wetMssEventReceiver">Receives processed MssEvents to be sent back to the host</param>
        /// <param name="mappingMgr">The MappingManager that will be used by mssMsgProcessor</param>
        public void Init(IDryMssEventEchoer dryMssEventEchoer, IWetMssEventReceiver wetMssEventReceiver, MappingManager mappingMgr)
        {
            this.mssMsgProcessor.Init(mappingMgr);

            this.wetMssEventReceiver = wetMssEventReceiver;

            dryMssEventEchoer.DryMssEventRecieved += new DryMssEventRecievedEventHandler(dryMssEventEchoer_DryMssEventRecieved);
        }

        /// <summary>
        ///     Event handler for MssEvents coming
        /// </summary>
        /// <param name="dryMssEvent"></param>
        protected void dryMssEventEchoer_DryMssEventRecieved(MssEvent dryMssEvent)
        {
            //Process in incoming MssEvent
            List<MssMsg> mssMessages = this.mssMsgProcessor.ProcessMssMsg(dryMssEvent.mssMsg);

            List<MssEvent> wetEventList = new List<MssEvent>(mssMessages.Count);
            //Convert the list of processed MssMsgs into an list of MssEvents.
            foreach (MssMsg mssMsg in mssMessages)
            {
                MssEvent wetEvent = new MssEvent();
                wetEvent.mssMsg = mssMsg;
                wetEvent.timestamp = dryMssEvent.timestamp;
                wetEventList.Add(wetEvent);
            }

            //Send the processed MssEvents to the WetMssEventRelay
            this.wetMssEventReceiver.ReceiveWetMssEventList(wetEventList);
        }
    }
}
