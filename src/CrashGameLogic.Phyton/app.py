from flask import Flask, jsonify
import random

app = Flask(__name__)

@app.route('/getOdds', methods=['GET'])
def get_odds():
    def logic_one():
        first = random.randint(0, 2**52)
        second = random.randint(0, 2**51)
        third = random.randint(0, 2**51)

        first = first / 2**52
        second = second / 2**52
        third = third / 2**52

        first_odd = 0.91 / (1 - first)

        if first_odd < 1:
            first_odd = 1
            second_odd = (2/3) / (1 - second)
            third_odd = (1/3) / ((1 - second) * (1 - third))
        else:
            second_odd = 0.607 / ((1 - first) * (1 - second))
            third_odd = 0.304 / ((1 - first) * (1 - second) * (1 - third))

        return float("%.2f" % first_odd), float("%.2f" % second_odd), float("%.2f" % third_odd)
        
    first_odd, second_odd, third_odd = logic_one()
    return jsonify({"first_odd": first_odd, "second_odd": second_odd, "third_odd": third_odd})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
