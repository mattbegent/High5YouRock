﻿using System;
using h5yr.Data.Migrations;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace h5yr.Core.Composers
{
    public class TweetCounterStoreComposer : ComponentComposer<TweetCounterStoreComponent>, IComposer
    {
    }

    public class TweetCounterStoreComponent : IComponent
    {
        private readonly ICoreScopeProvider _coreScopeProvider;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger<TweetCounterStoreComponent> _logger;
        private readonly IRuntimeState _runtimeState;

        public TweetCounterStoreComponent(ICoreScopeProvider coreScopeProvider,
            IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService,
            ILogger<TweetCounterStoreComponent> logger, IRuntimeState runtimeState)
        {
            _coreScopeProvider = coreScopeProvider ?? throw new ArgumentNullException(nameof(coreScopeProvider));
            _migrationPlanExecutor = migrationPlanExecutor ?? throw new ArgumentNullException(nameof(migrationPlanExecutor));
            _keyValueService = keyValueService ?? throw new ArgumentNullException(nameof(keyValueService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _runtimeState = runtimeState ?? throw new ArgumentNullException(nameof(runtimeState));
        }

        public void Initialize()
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            var migrationPlan = new MigrationPlan("TweetCounterCreateTableMigrationPlan1.2");

            migrationPlan = migrationPlan.From(string.Empty)
                .To<TweetCounterCreateTableMigration>(nameof(TweetCounterCreateTableMigration));

            var upgrader = new Upgrader(migrationPlan);

            upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
        }

        public void Terminate()
        {
        }
    }
}