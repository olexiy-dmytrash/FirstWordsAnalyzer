#!/bin/bash

# Script to run SQL queries against FirstWordsAnalyzer database
# Usage: ./run_sql.sh "SQL_QUERY" or ./run_sql.sh sql_file.sql

DB_SERVER="localhost"
DB_NAME="FirstWordsAnalyzer"
DB_USER="sa"
DB_PASSWORD="conor"

if [ $# -eq 0 ]; then
    echo "Usage: $0 \"SQL_QUERY\" or $0 sql_file.sql"
    exit 1
fi

# Check if argument is a file
if [ -f "$1" ]; then
    # Copy file to Windows accessible location
    cp "$1" /mnt/c/temp/query.sql
    cmd.exe /c "sqlcmd -S $DB_SERVER -d $DB_NAME -U $DB_USER -P $DB_PASSWORD -i C:\\temp\\query.sql"
else
    # Create temporary file with the query
    echo "$1" > /mnt/c/temp/query.sql
    cmd.exe /c "sqlcmd -S $DB_SERVER -d $DB_NAME -U $DB_USER -P $DB_PASSWORD -i C:\\temp\\query.sql"
fi