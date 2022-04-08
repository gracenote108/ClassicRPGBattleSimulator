using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using BattleSimulator;

namespace CombatQueue
{
    public class Battle
    {
        private Thread cqThread;
        private List<IParticipant> _participants;
        private List<BattleParticipant> _combatants;

        private CombatQueue _combatQueue;
        private List<IParticipant> _battleReadyList;
        private TextBlock _battleReadyDisplay;
        private TextBlock _combatQueueDisplay;

        private readonly Grid _grid;

        private void StartCQThread()
        {
            cqThread = new Thread(ProcessCombatQueue);
            cqThread.Start();

        }

        private void ProcessCombatQueue()
        {
            while (true)
            {
                
            }
        }

        public Battle(MainWindow window)
        {
            _grid = window.battleGrid;
            _participants = new List<IParticipant>()
            {
                new Fighter("Cloud", 33, "cloudAtt"),
                new Fighter("Tifa", 41, "tifaAtt"),
                new Fighter("Barret", 25, "barAtt"),
                new Fighter("Yuffie", 43, "yuffAtt")
            };

            _combatants = new List<BattleParticipant>();
            _battleReadyList = new List<IParticipant>();
            _combatQueue = new CombatQueue();

            CreateGrid();
            PopulateCharacters();

            var rqlabel = new TextBlock()
            {
                Text="Ready: "
            };
            Grid.SetColumn(rqlabel, 0);
            Grid.SetRow(rqlabel,4);
            Grid.SetColumnSpan(rqlabel, 5);
            _grid.Children.Add(rqlabel);
            
            var cqlabel = new TextBlock()
            {
                Text="Combat Queue: "
            };
            Grid.SetColumn(cqlabel, 0);
            Grid.SetRow(cqlabel, 5);
            Grid.SetColumnSpan(rqlabel, 5);
            _grid.Children.Add(cqlabel);

            _battleReadyDisplay = new TextBlock();
            Grid.SetColumn(_battleReadyDisplay, 1);
            Grid.SetRow(_battleReadyDisplay, 4);
            _grid.Children.Add(_battleReadyDisplay);

            _combatQueueDisplay = new TextBlock();
            Grid.SetColumn(_combatQueueDisplay, 1);
            Grid.SetRow(_combatQueueDisplay, 5);
            _grid.Children.Add(_combatQueueDisplay);

        }

        private void PopulateCharacters()
        {
            for (var i = 0; i < _participants.Count; i++)
            {
                var participant = _participants[i];
                var batPart = new BattleParticipant(participant);
                batPart.AddGrid(_grid, 0, i);
                _combatants.Add(batPart);
                batPart.ProgressBarFull += AddToReadyList;
                batPart.AttackExecuted += AddCombatQueue;
                batPart.LimitBreakExecuted += AddCombatQueue;
            }
        }

        private void AddToReadyList(object sender, IParticipant fighter)
        {
            _battleReadyList.Add(fighter);
            UpdateReadyList();
        }

        private void AddCombatQueue(object sender, IParticipant fighter)
        {
            _battleReadyList.Remove(fighter);
            _combatQueue.Enqueue(fighter);
            _combatQueueDisplay.Text = "";
            UpdateReadyList();
            foreach (IParticipant f in _combatQueue)
            {
                _combatQueueDisplay.Text += $"{f.Name} ";
            }
        }

        private void UpdateReadyList()
        {
            _battleReadyDisplay.Text = "";
            foreach (var p in _battleReadyList)
            {
                _battleReadyDisplay.Text += $"{p.Name} ";
            }
        }

        private void CreateGrid()
        {
            for (var i = 0; i <= 10; i++)
            {
                var newCol = new ColumnDefinition()
                {
                    Width = GridLength.Auto
                };
                _grid.ColumnDefinitions.Add(newCol);
            }

            for (var i = 0; i <= 10; i++)
            {
                var newRow = new RowDefinition()
                {
                    Height = new GridLength(20)
                };
                _grid.RowDefinitions.Add(newRow);
            }

        }
    }
}
