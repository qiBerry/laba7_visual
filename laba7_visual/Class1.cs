using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace laba7_visual
{
    class StateMachine
    {
        TextBox validator_text;
        TextBox status_text;
        Button card_button;
        public StateMachine(TextBox validator_text, TextBox status_text, Button card_button)
        {
            this.validator_text = validator_text;
            this.status_text = status_text;
            this.card_button = card_button;
            state = State.Init;
            this.validator_text.Text = "Здравствуйте\r\nПриложите карту для оплаты";
            this.status_text.Text = "Init";
        }

        private enum State { Init, ReadingCardInfo, CheckingBalance, DetectingHuman, OpeningDoors, ClosingDoors, Exit }

        private State state = State.Init;
        private State prevState;

        String text;
        String state_text;

        public void stateInit()
        {
            setState(State.Init);
            TextPrint();
            if (!prevState.Equals(State.DetectingHuman))
                stateReadingCardInfo();
        }

        void stateReadingCardInfo()
        {
            setState(State.ReadingCardInfo);
            TextPrint();
            stateCheckingBalance();
        }

        void stateCheckingBalance()
        {
            setState(State.CheckingBalance);
            TextPrint();
            Random random = new Random(100);
            if (random.Next() <= 50)
                stateInit();
            else
                stateDetectingHuman();

        }

        void stateDetectingHuman()
        {
            setState(State.DetectingHuman);
            TextPrint();
            if (prevState.Equals(State.CheckingBalance))
                stateOpeningDoors();
            else if (prevState.Equals(State.ClosingDoors))
            {
                stateInit();
                card_button.Invoke((MethodInvoker)delegate {
                    card_button.Enabled = true;
                });
            }
        }

        void stateOpeningDoors()
        {
            setState(State.OpeningDoors);
            TextPrint();
            stateClosingDoors();
        }

        void stateClosingDoors()
        {
            setState(State.ClosingDoors);
            TextPrint();
            stateDetectingHuman();
        }

        void setState(State value)
        {
            Thread.Sleep(3000);

            prevState = state;
            state = value;
        }


        void TextPrint()
        {
            switch (state)
            {
                case State.Init:
                    state_text = "Init";

                    if (prevState.Equals(State.CheckingBalance))
                        text = "Недостатоно средств\r\nПриложите другую карту";
                    else if (prevState.Equals(State.DetectingHuman) || prevState.Equals(State.Init))
                        text = "Здравствуйте\r\nПриложите карту для оплаты";
                    break;
                case State.ReadingCardInfo:
                    state_text = "ReadingCardInfo";

                    if (prevState.Equals(State.Init))
                        text = "Считывается информация с карты";
                    break;
                case State.CheckingBalance:
                    state_text = "CheckingBalance";

                    if (prevState.Equals(State.ReadingCardInfo))
                        text = "Данные карты считаны\r\nИдёт проверка баланса";
                    break;
                case State.DetectingHuman:
                    state_text = "DetectingHuman";

                    if (prevState.Equals(State.CheckingBalance))
                        text = "Поездка успешно оплачена\r\nПроизводится установление личности";
                    else if (prevState.Equals(State.ClosingDoors))
                        text = "Пожалуйста, подождите";
                    break;
                case State.OpeningDoors:
                    state_text = "OpeningDoors";

                    if (prevState.Equals(State.DetectingHuman))
                        text = "Личность установлена\r\nДвери открываются";
                    break;
                case State.ClosingDoors:
                    state_text = "ClosingDoors";

                    if (prevState.Equals(State.OpeningDoors))
                        text = "Двери закрываются";
                    break;
            }

            validator_text.Invoke((MethodInvoker)delegate {
                validator_text.Text = text;
            });

            status_text.Invoke((MethodInvoker)delegate {
                status_text.Text = state_text;
            });
        }
    }
}
