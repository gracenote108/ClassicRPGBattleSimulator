using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator
{
    public class CombatQueue: IEnumerator, IEnumerable
    {
        private FighterNode FirstNode = null;
        private FighterNode CurrentNode = null;
        private volatile int _count = 0;

        public int Count
        {
            get { return _count; }
        }
        public CombatQueue()
        {
            
        }

        public string Print()
        {
            var current = FirstNode;
            var currentQ = new List<string>();

            while (current != null)
            {
                currentQ.Add(current.fighter.Name);
                current = current.next;
            }
            return currentQ.ToString();
        }

        public void Enqueue(IParticipant fighter)
        {
            int num = 0;
            var newNode = new FighterNode(fighter);

            var current = FirstNode;
            if (FirstNode == null)
            {
                FirstNode = newNode;
                _count++;
                return;
            }
            
            while (current.next != null)
                current = current.next;

            current.next = newNode;
            _count++;
        }

        public IParticipant Dequeue()
        {

            var current = FirstNode;
            FirstNode = current.next;
            _count--;

            return current.fighter;

        }

        public void Reverse()
        {
            FighterNode previous = null;
            FighterNode next = null;
            var current = FirstNode;

            while (current != null)
            {
                next = current.next;
                current.next = previous;
                previous = current;
                current = next;
            }

            FirstNode = previous;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator) this;
        }

        public bool MoveNext()
        {
            if (CurrentNode == null)
            {
                CurrentNode = FirstNode;
                return true;
            }
            CurrentNode = CurrentNode.next;
            return CurrentNode != null;
        }

        public void Reset()
        {
            CurrentNode = FirstNode;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public IParticipant Current
        {
            get
            {
                return CurrentNode.fighter;
            }
        }

        private class FighterNode
        {
            public int Number;
            public FighterNode next = null;
            public IParticipant fighter;

            public FighterNode(IParticipant prFighter)
            {
                fighter = prFighter;
            }
            
        }

    }
}
