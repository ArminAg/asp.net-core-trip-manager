﻿using asp.net_core_trip_manager.Core.Repositories;
using asp.net_core_trip_manager.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TripContext _context;
        private ILogger<TripRepository> _tripLogger;
        private ILogger<StopRepository> _stopLogger;

        public ITripRepository Trips { get; private set; }
        public IStopRepository Stops { get; private set; }

        public UnitOfWork(TripContext context, ILogger<TripRepository> tripLogger, ILogger<StopRepository> stopLogger)
        {
            _context = context;
            _tripLogger = tripLogger;
            _stopLogger = stopLogger;
            Trips = new TripRepository(_context, _tripLogger);
            Stops = new StopRepository(_context, _stopLogger);
        }

        public async Task<bool> CompleteAsync()
        {
            return await (_context.SaveChangesAsync()) > 0;
        }
    }
}
