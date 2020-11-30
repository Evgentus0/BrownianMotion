using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrownianMotion
{
    class Crystal
    {
        private readonly int _n;
        private readonly int _k;
        private readonly double _p;
        private readonly bool _withLock;
        private readonly List<int> _crystal;

        private object _locker = new object();

        public List<int> CrystalState => _crystal;

        public Crystal(int n, int k, double p, bool withLock)
        {
            _n = n;
            _k = k;
            _p = p;
            _withLock = withLock;
            _crystal = new List<int>(new int[n]);
            _crystal[0] = k;
        }

        public void StartSimulation(CancellationToken stop)
        {
            for(int i = 0; i < _k; i++)
            {
                var atom = new Atom(0, _n, _p);
                atom.AtomEvent += UpdateCrystalState;
                var thread = new Thread(() => atom.StartMove(stop));
                thread.Start();
            }
        }

        private void UpdateCrystalState(object sender, AtomEventArgs e)
        {
            if (_withLock)
            {
                UpdateWithLock(e);
            }
            else
            {
                UpdateWithoudLock(e);
            }
        }

        private void UpdateWithLock(AtomEventArgs e)
        {
            lock (_locker)
            {
                _crystal[e.PrevIndex] -= 1;
                _crystal[e.NewIndex] += 1;
            }
        }

        private void UpdateWithoudLock(AtomEventArgs e)
        {
            _crystal[e.PrevIndex] -= 1;
            _crystal[e.NewIndex] += 1;
        }
    }
}
