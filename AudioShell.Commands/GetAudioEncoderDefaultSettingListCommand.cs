﻿/*
 * Copyright © 2014 Jeremy Herbison
 * 
 * This file is part of AudioShell.
 * 
 * AudioShell is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 * 
 * AudioShell is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more
 * details.
 * 
 * You should have received a copy of the GNU Lesser General Public License along with AudioShell.  If not, see
 * <http://www.gnu.org/licenses/>.
 */

using AudioShell.Commands.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.Management.Automation;

namespace AudioShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "AudioEncoderDefaultSettingList"), OutputType(typeof(SettingsDictionary))]
    public class GetAudioEncoderDefaultSettingListCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Encoder { get; set; }

        protected override void ProcessRecord()
        {
            var encoderFactory = ExtensionProvider<ISampleEncoder>.Instance.Factories.Where(factory => string.Compare((string)factory.Metadata["Name"], Encoder, StringComparison.OrdinalIgnoreCase) == 0).SingleOrDefault();
            if (encoderFactory == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.EncoderUnknownError, Encoder));

            using (var encoderLifetime = encoderFactory.CreateExport())
                WriteObject(encoderLifetime.Value.DefaultSettings, true);
        }
    }
}