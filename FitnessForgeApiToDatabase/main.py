import requests
import json
import pymysql


def insert_exercise(conn, exercise_data):
    cursor = conn.cursor()

    # Extract data from exercise_data
    name = exercise_data.get('name')
    instructions = exercise_data.get('instructions')
    exercise_type = exercise_data.get('type')
    muscle = exercise_data.get('muscle')
    equipment = exercise_data.get('equipment')

    if name:
        # Check if exercise with the same name exists
        cursor.execute("SELECT Id FROM exercise WHERE Name = %s", (name,))
        row = cursor.fetchone()
        if row:
            exercise_id = row[0]
            print(f"Exercise '{name}' already exists in the database.")
        else:
            # Insert exercise into 'exercise' table
            cursor.execute("INSERT INTO exercise (Name, Instructions, TypeId) VALUES (%s, %s, (SELECT Id FROM exercise_type WHERE Name = %s))",
                           (name, instructions, exercise_type))
            exercise_id = cursor.lastrowid
            print(f"Exercise '{name}' inserted into the database.")

        # Insert or get muscle ID
        if muscle:
            cursor.execute("SELECT Id FROM muscle WHERE Name = %s", (muscle,))
            row = cursor.fetchone()
            if row:
                muscle_id = row[0]
            else:
                cursor.execute("INSERT INTO muscle (Name) VALUES (%s)", (muscle,))
                muscle_id = cursor.lastrowid

            # Check if the entry already exists in the linking table
            cursor.execute("SELECT * FROM exercise_trains_muscle WHERE ExerciseId = %s AND MuscleId = %s",
                           (exercise_id, muscle_id))
            if not cursor.fetchone():
                # Insert into linking table
                cursor.execute("INSERT INTO exercise_trains_muscle (ExerciseId, MuscleId) VALUES (%s, %s)",
                               (exercise_id, muscle_id))

        # Insert or get equipment ID
        if equipment:
            cursor.execute("SELECT Id FROM equipment WHERE Name = %s", (equipment,))
            row = cursor.fetchone()
            if row:
                equipment_id = row[0]
            else:
                cursor.execute("INSERT INTO equipment (Name) VALUES (%s)", (equipment,))
                equipment_id = cursor.lastrowid

            # Check if the entry already exists in the linking table
            cursor.execute("SELECT * FROM exercise_requires_equipment WHERE ExerciseId = %s AND EquipmentId = %s",
                           (exercise_id, equipment_id))
            if not cursor.fetchone():
                # Insert into linking table
                cursor.execute("INSERT INTO exercise_requires_equipment (ExerciseId, EquipmentId) VALUES (%s, %s)",
                               (exercise_id, equipment_id))

        conn.commit()
    else:
        print("Error: 'name' missing in exercise_data")





conn = pymysql.connect(
    host='ho.m108.eu',
    user='m108-ho',
    password='aIy0bXw80cNKIcJ0rgND',
    database='m108ho'
)

offsets = [0, 10]
types = [
    'cardio',
    'olympic_weightlifting',
    'plyometrics',
    'powerlifting',
    'strength',
    'stretching',
    'strongman'
]

for exercise_type in types:
    for offset in offsets:
        api_url = 'https://api.api-ninjas.com/v1/exercises?type={}&offset={}'.format(exercise_type, offset)
        response = requests.get(api_url, headers={'X-Api-Key': 'vfiCuJNFtgCiOoHXPaGVUrjELSZyWCJndcZfDVNF'})

        if response.status_code == requests.codes.ok:
            api_response = response.json()
            for exercise_data in api_response:
                insert_exercise(conn, exercise_data)
        else:
            print("Error:", response.status_code, response.text)

