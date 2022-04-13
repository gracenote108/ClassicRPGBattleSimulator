using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace BattleSimulator
{
    public class Battle
    {
        private CombatQueueManager _combatQueueManager;
        private List<IParticipant> _participants;
        private List<BattleParticipant> _combatants;

        private List<IParticipant> _battleReadyList;
        private TextBlock _battleReadyDisplay;
        private TextBlock _combatQueueDisplay;
        private volatile TextBlock _battleComments;

        private readonly Grid _grid;

        private void StartCQThread()
        {

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

            _battleComments = new TextBlock();
            Grid.SetColumn(_battleComments, 1);
            Grid.SetRow(_battleComments, 6);
            _battleComments.Text = "Battle Comments Here";
            _grid.Children.Add(_battleComments);

            //This probably needs to be organized better...probably
            _combatQueueManager = new CombatQueueManager(_battleComments, _combatQueueDisplay);
            _combatQueueManager.StartQueueWatch();

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
            lock (_battleReadyList)
            {
                _battleReadyList.Add(fighter);
                UpdateReadyList();
            }
            
        }

        private async void AddCombatQueue(object sender, IParticipant fighter)
        {
            lock (_battleReadyList)
            {
                _battleReadyList.Remove(fighter);
                UpdateReadyList();
            }
            await _combatQueueManager.SafeEnqueue(fighter);
            await _combatQueueManager.UpdateCombatQueueList();
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
