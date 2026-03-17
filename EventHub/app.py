import json
import random
import time
from datetime import datetime

from azure.eventhub import EventHubProducerClient, EventData
from azure.identity import ClientSecretCredential


# ---------------------------
# AZURE CONFIGURATION
# ---------------------------

TENANT_ID = 
CLIENT_ID = 
CLIENT_SECRET = 

EVENT_HUB_NAMESPACE = "demoeventhub1038.servicebus.windows.net"
EVENT_HUB_NAME = "demo"


# ---------------------------
# AUTHENTICATION
# ---------------------------

credential = ClientSecretCredential(
    tenant_id=TENANT_ID,
    client_id=CLIENT_ID,
    client_secret=CLIENT_SECRET
)


producer = EventHubProducerClient(
    fully_qualified_namespace=EVENT_HUB_NAMESPACE,
    eventhub_name=EVENT_HUB_NAME,
    credential=credential
)


# ---------------------------
# RANDOM JSON GENERATOR
# ---------------------------

def generate_random_json():
    data = {
        "device_id": f"device-{random.randint(1,50)}",
        "temperature": round(random.uniform(20, 40), 2),
        "humidity": round(random.uniform(30, 90), 2),
        "status": random.choice(["running", "idle", "error"]),
        "timestamp": datetime.utcnow().isoformat()
    }

    return data


# ---------------------------
# SEND EVENTS
# ---------------------------

def send_events():

    with producer:
        while True:

            event_data_batch = producer.create_batch()

            for _ in range(5):

                json_payload = generate_random_json()

                event = EventData(json.dumps(json_payload))
                
                batch = producer.create_batch(partition_key=json_payload["device_id"])

                batch.add(event)

                print("Sending:", json_payload)

            producer.send_batch(batch)

            time.sleep(5)


if __name__ == "__main__":
    send_events()