﻿// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2019-present, Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Developer Agreement, located
// here: https://auth.magicleap.com/terms/developer
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using UnityEngine;

namespace MagicLeapTools
{
    public class GlobalBoolsRecapMessage : TransmissionMessage
    {
        //Public Variables(truncated to reduce packet size):
        /// <summary>
        /// keys
        /// </summary>
        public string[] k;
        /// <summary>
        /// values
        /// </summary>
        public bool[] v;

        //Constructors:
        public GlobalBoolsRecapMessage(string address, string[] keys, bool[] values) : base(TransmissionMessageType.GlobalBoolsRecapMessage, TransmissionAudience.SinglePeer, address, true)
        {
            k = keys;
            v = values;
        }
    }
}