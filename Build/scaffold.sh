cd ../TodoApi
dotnet ef dbcontext scaffold "Host=localhost;Database=antoine.dray;Username=antoine.dray;Password=password" Npgsql.EntityFrameworkCore.PostgreSQL -d -f -c DataContext -o Models --verbose