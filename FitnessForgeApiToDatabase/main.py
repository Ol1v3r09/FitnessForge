import requests
import pymysql
from googletrans import Translator

conn = pymysql.connect(
    host='localhost',
    user='root',
    password='1234',
    database='fitnessforge'
)

def insert_exersice():
    cursor = conn.cursor()

    ex_type = ""
    muscle = ""
    offset = 0

    api_key = 'Sgd5m0oeNrrxu6DAE2h2QA==grADgHsByZmYU4Lu'

    api_url = 'https://api.api-ninjas.com/v1/exercises?'
    headers = {'X-Api-Key': api_key}
    response = requests.get(api_url, headers=headers)

    if response.status_code == requests.codes.ok:
        data = response.json()

    else:
        print("Error:", response.status_code, response.text)

    conn.commit()
    conn.close()

def insert_muscles():
    cursor = conn.cursor()

    muscles = [
        "Mellizom (pectoralis major)",
        "Hasizom (abdominals)",
        "Farizom (gluteus maximus)",
        "Hátsó combizom (hamstrings)",
        "Négyfejű combizom (vastus lateralis, vastus medialis, vastus intermedius, rectus femoris)",
        "Vádli (calf muscles)",
        "Hátizom (latissimus dorsi)",
        "Hátsó vállizom (posterior deltoid)",
        "Elülső vállizom (anterior deltoid)",
        "Középső vállizom (medial deltoid)",
        "Tricepsz (triceps brachii)",
        "Bicepsz (biceps brachii)",
        "Széles hátizom (trapezius)",
        "Alsó hátizom (erector spinae)",
        "Alkarizom (flexor carpi radialis, flexor carpi ulnaris, extensor carpi radialis longus, extensor carpi radialis brevis, extensor carpi ulnaris)",
        "Nyakizom (sternocleidomastoid, trapezius)",
        "Alsó lábizom (soleus)",
        "Kézhátizom (dorsal interosseous, palmar interosseous)",
        "Középső törzsizom (rectus abdominis)",
        "Alsó hasizom (transversus abdominis)"
    ]
    for muscle in muscles:
        sql_insert = "INSERT INTO muscle (Name) VALUES (%s)"
        cursor.execute(sql_insert, muscle)


    conn.commit()
    conn.close()

def insert_types():
    cursor = conn.cursor()

    types = [
        'kardió',
        'olimpiai súlyemelés',
        'pliometria',
        'erőemelés',
        'erő',
        'nyújtás',
        'erőember'
    ]
    for type in types:
        sql_insert = "INSERT INTO exercise_type (Name) VALUES (%s)"
        cursor.execute(sql_insert, type)


    conn.commit()
    conn.close()

insert_muscles()