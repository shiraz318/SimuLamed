"""
Parse the html in column Description, and fill columns
Ans_1, Ans_2, Ans_3, Ans_4, Correct_Ans and Image if there is.
"""
import csv

# Constants.
from pip._vendor import requests

QUESTION_IDX = 0
DESCRIPTION_IDX = 1
ANS_1_IDX = 3
ANS_2_IDX = 4
ANS_3_IDX = 5
ANS_4_IDX = 6
CORRECT_ANS_IDX = 7
IMAGE_IDX = 8


def get_image(description):
    if "img" in description:
        start = description.split("img src=")[1]
        middle = start.split(" ")[0]
        return middle
    else:
        return ""


def pars_ans(description):
    ans_1 = ans_2 = ans_3 = ans_4 = correct_ans = ""
    answers = [ans_1, ans_2, ans_3, ans_4, correct_ans]
    counter = 0
    is_correct = False
    for item in description.split("<span"):
        if "</span>" not in item:
            continue
        inner_item = item.split("</span>")[0]

        if inner_item.startswith(" id"):
            answers[4] = inner_item.split(">")[1]
            is_correct = True

        if is_correct:
            answers[counter] = inner_item.split(">")[1]
        else:
            answers[counter] = inner_item[1:]
        counter += 1
        if counter == 4:
            break

    return answers[0], answers[1], answers[2], answers[3], answers[4]


def parse_content():
    is_first_row = True
    with open('Relevant_Cols_Temp.csv', 'rt', encoding="utf8") as inp, open('Relevant_Cols.csv', 'wt', encoding="utf8",
                                                                            newline='') as out:
        writer = csv.writer(out)
        for row in csv.reader(inp):

            if is_first_row:
                is_first_row = False
                continue

            # Remove the question number.
            row[QUESTION_IDX] = row[QUESTION_IDX][5:]
            description = row[DESCRIPTION_IDX]
            ans_1, ans_2, ans_3, ans_4, correct_ans = pars_ans(description)
            row[ANS_1_IDX] = ans_1
            row[ANS_2_IDX] = ans_2
            row[ANS_3_IDX] = ans_3
            row[ANS_4_IDX] = ans_4
            row[CORRECT_ANS_IDX] = correct_ans
            image = get_image(description)
            row[IMAGE_IDX] = image
            writer.writerow(row)

    out.close()
    inp.close()


def download_images():
    with open('pic1.jpg', 'wb') as handle:
        response = requests.get("https://www.gov.il/BlobFolder/generalpage/tq_pic_01/he/TQ_PIC_31073.jpg", stream=True)

        if not response.ok:
            print(response)

        for block in response.iter_content(1024):
            if not block:
                break

            handle.write(block)
    # f = open('1.jpg', 'wb')
    # f.write(requests.get("https://www.gov.il/BlobFolder/generalpage/tq_pic_01/he/TQ_PIC_31073.jpg").content)
    # f.close()



def main():
    parse_content()
    # download_images()

if __name__ == '__main__':
    main()
