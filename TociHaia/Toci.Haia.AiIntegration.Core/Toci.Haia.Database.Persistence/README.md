# Toci.Haia.Database.Persistence

EF Core DB-first persistence layer for HAIA PostgreSQL database.

## Source of truth
Database schema is the source of truth. Do not create EF migrations in this project.

## Security
- Do not store credentials in code or configuration files.
- Use environment variable `HAIA_DB_CONNECTION_STRING`.
- Safe placeholder example:
  - `Host=localhost;Port=5433;Database=Toci.Haia;Username=postgres;Password=<SET_LOCALLY>`

## Regeneration
Run from solution root:

```powershell
./Toci.Haia.Database.Persistence/Tools/Scaffold-HaiaDatabase.ps1
```

The script:
- restores local tools,
- scaffolds only HAIA schemas,
- uses `--no-onconfiguring`,
- uses `--force` against Generated folders,
- builds the project,
- validates that generated source does not contain forbidden password fragments.

## Notes
- Manual code must stay outside `Entities/Generated` and `Context/Generated`.
- Triggers/functions/check constraints remain enforced in PostgreSQL.
