from flask import Flask, jsonify
import random

app = Flask(__name__)

@app.route('/getOdds', methods=['GET'])
def get_odds():
    def logic_one():
        

        return float("%.2f" % 1.1), float("%.2f" % 1.1), float("%.2f" % 1.1)
        
    first_odd, second_odd, third_odd = logic_one()
    return jsonify({"first_odd": first_odd, "second_odd": second_odd, "third_odd": third_odd})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
