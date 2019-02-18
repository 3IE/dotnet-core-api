cd ../TodoApi

if [ "$1" = "--3ie" ]; then
    DB="antoine.dray"
    USER="antoine.dray"
elif [[ "$1" = "--Home" ]]; then
    DB="todo"
    USER="antoinedray"
else
    echo "Usage: ./scaffold.sh [env]"
    exit 1
fi

dotnet ef dbcontext scaffold "Host=localhost;Database=${DB};Username=${USER};Password=password" Npgsql.EntityFrameworkCore.PostgreSQL -d -f -c DataContext -o Entities --verbose