using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Sango.Game.Player
{
    [GameSystem(auto = true)]
    public class PlayerMessage : GameSystem
    {
        int maxSaveCount = 100;
        string windowName = "window_player_message";
        bool newDay = true;
        static PlayerMessage Instance;

        public class TextMessage
        {
            public string text;
            public Force force;
            public int x;
            public int y;
            public int year;
            public int month;
            public int day;
        }

        public class PersonMessage
        {
            public Person person;
            public string text;
        }

        List<TextMessage> textMessages = new List<TextMessage>();
        List<PersonMessage> personMessages = new List<PersonMessage>();
        Scenario scenario;

        public override void Init()
        {
            GameEvent.OnScenarioInit += OnScenarioInit;
            GameEvent.OnDayUpdate += OnDayUpdate;
        }
        public override void Clear()
        {
            GameEvent.OnScenarioInit += OnScenarioInit;
            GameEvent.OnDayUpdate -= OnDayUpdate;
        }


        void OnDayUpdate(Scenario scenario)
        {
            newDay = true;
        }


        void OnScenarioInit(Scenario scenario)
        {
            textMessages.Clear();
            personMessages.Clear();
            this.scenario = scenario;
            // 更新实例
            Instance = GameSystem.GetSystem<PlayerMessage>();
        }

        void _AddTextMessage(string text, Force force, int x, int y)
        {
            TextMessage message = new TextMessage()
            {
                text = text,
                force = force,
                x = x,
                y = y
            };

            if (newDay)
            {
                message.year = scenario.Info.year;
                message.month = scenario.Info.month;
                message.day = scenario.Info.day;
                newDay = false;
            }

            if (textMessages.Count >= maxSaveCount)
                textMessages.RemoveAt(0);
            textMessages.Add(message);
        }

        public static void AddTextMessage(string text, Force force, int x, int y)
        {
            Instance._AddTextMessage(text, force, x, y);
        }

        void _AddPersonMessage(string text, Person person)
        {
            PersonMessage message = new PersonMessage()
            {
                text = text,
                person = person,
            };
            if (personMessages.Count >= maxSaveCount)
                personMessages.RemoveAt(0);
            personMessages.Add(message);
        }
        public static void AddPersonMessage(string text, Person person)
        {
            Instance._AddPersonMessage(text, person);
        }
    }
}
