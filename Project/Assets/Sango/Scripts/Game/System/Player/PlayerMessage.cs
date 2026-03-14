using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Sango.Game.Player
{
    [GameSystem]
    public class PlayerMessage : GameSystem
    {
        public delegate void PlayerTextMessageCallback(TextMessage msg, PlayerMessage message);
        public delegate void PlayerPersonMessageCallback(PersonMessage msg, PlayerMessage message);

        int maxSaveCount = 100;
        string windowName = "window_player_message";
        bool newDay = true;
        static PlayerMessage Instance;

        public System.Action<bool> onVisibleChange;

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

        public List<TextMessage> textMessages = new List<TextMessage>();
        public List<PersonMessage> personMessages = new List<PersonMessage>();
        Scenario scenario;

        public PlayerTextMessageCallback onTextMessageAdd;
        public PlayerPersonMessageCallback onPersonMessageAdd;

        public override void Init()
        {
            GameEvent.OnScenarioInit += OnScenarioInit;
            GameEvent.OnDayUpdate += OnDayUpdate;
            GameEvent.OnScenarioStart += OnScenarioStart;
            GameEvent.OnScenarioEnd += OnScenarioEnd;

        }
        public override void Clear()
        {
            GameEvent.OnScenarioInit += OnScenarioInit;
            GameEvent.OnDayUpdate -= OnDayUpdate;
            GameEvent.OnScenarioStart -= OnScenarioStart;
            GameEvent.OnScenarioEnd -= OnScenarioEnd;
        }

        void OnScenarioStart(Scenario scenario)
        {
            Window.Instance.Open(windowName);
        }

        void OnScenarioEnd(Scenario scenario)
        {
            Window.Instance.Close(windowName);
        }

        public void SetVisible(bool b)
        {
            Window.Instance.SetVisible(windowName, b);
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
            TextMessage message;
            if (textMessages.Count >= maxSaveCount)
            {
                message = textMessages[0];
                textMessages.RemoveAt(0);
                message.text = text;
                message.force = force;
                message.x = x; message.y = y;
            }
            else
            {
                message = new TextMessage()
                {
                    text = text,
                    force = force,
                    x = x,
                    y = y
                };
            }

            if (newDay)
            {
                message.year = scenario.Info.year;
                message.month = scenario.Info.month;
                message.day = scenario.Info.day;
                newDay = false;
            }

            textMessages.Add(message);
            onTextMessageAdd?.Invoke(message, this);
        }

        public static void AddTextMessage(string text, Force force, int x, int y)
        {
            Instance._AddTextMessage(text, force, x, y);
        }

        void _AddPersonMessage(string text, Person person)
        {
            PersonMessage message;
            if (personMessages.Count >= maxSaveCount)
            {
                message = personMessages[0];
                personMessages.RemoveAt(0);
                message.text = text;
                message.person = person;
            }
            else
            {
                message = new PersonMessage()
                {
                    text = text,
                    person = person,
                };
            }
            personMessages.Add(message);
            onPersonMessageAdd?.Invoke(message, this);
        }

        public static void AddPersonMessage(string text, Person person)
        {
            Instance._AddPersonMessage(text, person);
        }
    }
}
