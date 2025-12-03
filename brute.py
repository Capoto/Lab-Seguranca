import itertools
import string
import time

def brute_force_time(password, attempts_per_second=1_000_000):
    # Definição do conjunto de caracteres — depende da complexidade da senha
    charset = ""
    if any(c.islower() for c in password):
        charset += string.ascii_lowercase
    if any(c.isupper() for c in password):
        charset += string.ascii_uppercase
    if any(c.isdigit() for c in password):
        charset += string.digits
    if any(c in string.punctuation for c in password):
        charset += string.punctuation
    
    charset_size = len(charset)
    password_length = len(password)

    # Número total de combinações possíveis
    total_combinations = charset_size ** password_length

    # Estimativa de tempo
    seconds = total_combinations / attempts_per_second

    return total_combinations, seconds

def format_time(seconds):
    minute = 60
    hour = 3600
    day = 86400
    year = 86400 * 365

    if seconds < minute:
        return f"{seconds:.2f} segundos"
    elif seconds < hour:
        return f"{seconds/minute:.2f} minutos"
    elif seconds < day:
        return f"{seconds/hour:.2f} horas"
    elif seconds < year:
        return f"{seconds/day:.2f} dias"
    else:
        return f"{seconds/year:.2f} anos"

password = "admin"  # exemplo
total, seconds = brute_force_time(password, attempts_per_second=1_000_000)
print(f"Total de combinações possíveis: {total:,}")
print(f"Tempo estimado: {format_time(seconds)}")
