using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BattleSimulator
{
    public class BattleParticipant
    {
        private IParticipant participant;

        private StackPanel _container;
        private TextBlock _nameField;

        private StatusBar _statBar;
        private ProgressBar _progBar;

        private Button _atbBtn;
        private Button _atkBtn;
        private Button _limitBreak;

        public event EventHandler<IParticipant> ProgressBarFull;
        public event EventHandler<IParticipant> AttackExecuted;
        public event EventHandler<IParticipant> LimitBreakExecuted;

        private int progBarMax = 700;

        public BattleParticipant(IParticipant prParticipant)
        {
            participant = prParticipant;
            _container = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            // First Column - Character Names
            _nameField = new TextBlock()
            {
                Text = participant.Name,
            };
            _container.Children.Add(_nameField);
            
            // Second Column - ATB Bar
            _statBar = new StatusBar();
            _progBar = new ProgressBar()
            {
                Width = 100,
                Height = 12,
                Value = 0,
                Maximum = progBarMax,
                Name = participant.AttributeName
            };
            _statBar.Items.Add(_progBar);
            _container.Children.Add(_statBar);

            // ATB Manual Trigger
            _atbBtn = new Button()
            {
                Content = "ATB"
            };
            _container.Children.Add(_atbBtn);
            _atbBtn.Click += async (sender, e) => await StartCount(sender, e);
            

            // Attack
            _atkBtn = new Button()
            {
                Content = "Attack",
                IsEnabled = false
            };
            _container.Children.Add(_atkBtn);
            _atkBtn.Click += Attack;

            // Limit Break
            _limitBreak = new Button()
            {
                Content = "Limit",
                IsEnabled = false
            };
            _container.Children.Add(_limitBreak);
            _limitBreak.Click += LimitBreak;

            Grid.SetColumnSpan(_container, 4);
        }

        protected virtual void OnProgressBarFull(IParticipant part)
        {
            ProgressBarFull?.Invoke(this, part);
        }

        protected virtual void OnAttackExecuted(IParticipant part)
        {
            AttackExecuted?.Invoke(this, part);
        }

        protected virtual void OnLimitBreakExecuted(IParticipant part)
        {
            LimitBreakExecuted?.Invoke(this, part);
        }

        public void AddGrid(Grid grid, int prColumnNum, int prRowNum)
        {
            Grid.SetColumn(_container, prColumnNum);
            Grid.SetRow(_container, prRowNum);
            grid.Children.Add(_container);
        }

        public async Task StartCount(object sender, RoutedEventArgs e)
        {
            _atbBtn.IsEnabled = false;
            for (int i = 0; i <= progBarMax; i+=participant.Speed)
            {
                await Task.Delay(new TimeSpan(0, 0, 0, 0, 50));
                _progBar.Value += participant.Speed;
            }

            _atkBtn.IsEnabled = true;
            _limitBreak.IsEnabled = true;
            OnProgressBarFull(participant);
        }

        public async void Attack(object sender, RoutedEventArgs e)
        {
            await Task.Delay(new TimeSpan(0, 0, 0, 0, 50));
            _atbBtn.IsEnabled = true;
            _progBar.Value = 0;
            _atkBtn.IsEnabled = false;
            _limitBreak.IsEnabled = false;
            OnAttackExecuted(participant);
        }
        public async void LimitBreak(object sender, RoutedEventArgs e)
        {
            await Task.Delay(new TimeSpan(0, 0, 0, 0, 50));
            _atbBtn.IsEnabled = true;
            _progBar.Value = 0;
            _atkBtn.IsEnabled = false;
            _limitBreak.IsEnabled = false;
            OnLimitBreakExecuted(participant);
        }
    }
}
