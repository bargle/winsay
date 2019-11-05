﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using CommandLine;
using CommandLine.Text;

namespace say
{
    class Options
    {
        [Option( 'm', "message", Required = false, HelpText = "Text to Speak" )]
        public string Message { get; set; }

        [Option( 'v', "voice", Required = false, HelpText = "TTS voice to use" )]
        public string TTSVoice { get; set; }

        [Option( 's', "speed", Required = false, HelpText = "Speed for TTS voice" )]
        public int TTSSpeed { get; set; }

        [Option( 'g', "gender", Required = false, HelpText = "Select TTS voice by gender" )]
        public string TTSGender { get; set; }

        [Option( 'i', "volume", Required = false, HelpText = "Volume of TTS voice" )]
        public int TTSVolume { get; set; }

        [Option( 'l', "lsvoices", Required = false, HelpText = "show list of installed TTS voices" )]
        public bool ListVoices { get; set; }

        [Option( 'o', "output", Required = false, HelpText = "Output to wave file" )]
        public string TTSFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("TTS Say Command Line Utility", "0.2"),
                Copyright = new CopyrightInfo("Michael-John, Updated by github/bargle", 2019),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine( "all rights reserved" );
            help.AddPreOptionsLine( "Usage: [Message] <other options>" );
            help.AddOptions( this );
            return help;
        }
    }

    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static InstalledVoice[] voiceList = synth.GetInstalledVoices().ToArray();

        public static Options options = new Options();
        public static void Main( String[] args )
        {

            if ( CommandLine.Parser.Default.ParseArguments( args, options ) )
            {
                if ( options.ListVoices )
                {
                    listVoices();
                }

                if ( options.TTSGender == "male" || options.TTSGender == "female" )
                {
                    VoiceGender genderMale = VoiceGender.Male;
                    VoiceGender genderFemale = VoiceGender.Female;
                    if ( options.TTSGender == "female" )
                    {
                        synth.SelectVoiceByHints( genderFemale );
                    }
                    else if ( options.TTSGender == "male" )
                    {
                        synth.SelectVoiceByHints( genderMale );
                    }
                }
                if ( options.TTSSpeed <= 10 && options.TTSSpeed >= -10 )
                {
                    synth.Rate = options.TTSSpeed;
                }
                if ( options.TTSVolume > 0 && options.TTSVolume <= 100 )
                {
                    synth.Volume = options.TTSVolume;
                }
                if ( options.TTSVoice != null )
                {
                    synth.SelectVoice( parseVoice( options.TTSVoice ) );
                }
                if ( options.TTSVoice != null )
                {
                    synth.SetOutputToWaveFile( options.TTSFile );
                }
                if ( options.Message != null )
                {
                    synth.Speak( parseMessage( options.Message ) );
                }
            }
        }

        public static void listVoices()
        {
            int i = 0;
            for ( i = 0; i < voiceList.Length; i++ )
            {
                Console.Out.WriteLine( voiceList[i].VoiceInfo.Name );
            }
        }

        public static string parseVoice( string input )
        {
            char underscore = '_';
            char space = ' ';
            String properVoiceName = input.Replace(underscore, space);

            int i = 0;
            for ( i = 0; i < voiceList.Length; i++ )
            {
                if ( properVoiceName == voiceList[i].VoiceInfo.Name )
                {
                    Console.Out.WriteLine( properVoiceName );
                    return properVoiceName;
                }
            }
            return null;
        }

        public static string parseMessage( string input )
        {
            char underscore = '_';
            char space = ' ';

            String properMessage = input.Replace(underscore, space);
            return properMessage;
        }
    }
}