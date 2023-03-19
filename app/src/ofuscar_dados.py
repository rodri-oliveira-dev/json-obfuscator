import json
from typing import Any, Set


class ObfuscateJson:
    def __init__(self, names: Set[str]):
        self.names = names

    def to_json(self, obj: Any) -> str:
        if obj is None:
            return "{}"

        try:
            json_str = json.dumps(
                obj, default=lambda o: o.__dict__, sort_keys=True)
            if not self.names:
                return json_str

            json_obj = json.loads(json_str)
            self.filter_json_element(json_obj)
            return json.dumps(
                json_obj,
                default=lambda o: o.__dict__,
                sort_keys=True)
        except json.JSONDecodeError:
            return "{}"

    def filter_json_element(self, json_element: Any) -> None:
        if isinstance(json_element, dict):
            for key, value in json_element.items():
                if key in self.names:
                    json_element[key] = self.replace_value(value)
                else:
                    self.filter_json_element(value)
        elif isinstance(json_element, list):
            for index, value in enumerate(json_element):
                json_element[index] = self.replace_value(value)

    def replace_value(self, value: Any) -> Any:
        if isinstance(value, (dict, list)):
            self.filter_json_element(value)
            return value
        elif isinstance(value, (str, int, float, bool)):
            return "oculto"
        else:
            return "oculto"
