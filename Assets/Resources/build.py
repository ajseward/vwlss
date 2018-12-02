from collections import Counter

medium_dict = "medium_length_dict.txt"
long_dict = "long_length_dict.txt"

easy = "easy.txt"
medium = "medium.txt"
hard = "hard.txt"
insane = "insane.txt"

full = "full.txt"

vowels = ['a', 'e', 'i', 'o', 'u']

max_legnth = 10

easy_max_length = 5
medium_max_length = 8
hard_min_length = 7

easy_max_vowels = 2
medium_max_vowels = 4
hard_min_vowels = 2



long_dict_file = open(long_dict, "r")
medium_dict_file = open(medium_dict, "r")

easy_file = open(easy, "w+")
medium_file = open(medium, "w+")
hard_file = open(hard, "w+")
insane_file = open(insane, "w+")
full_file = open(full, "w+")

full_file.writelines(medium_dict_file.readlines())
full_file.write("\n")
full_file.writelines(long_dict_file.readlines())

full_file.seek(0)

for line in full_file:
    ln = line.strip('\n')
    if ln.startswith(tuple(vowels)) or ln.endswith(tuple(vowels)):
        continue

    counter = Counter(ln)
    count = sum(counter.get(c, 0) for c in vowels)

    if len(ln) <= max_legnth:

        if count >= hard_min_vowels and len(ln) >= hard_min_length:
            print("[Hard]: " + ln)
            print("[Insane]: " + ln)
            hard_file.write(line)
            insane_file.write(line)

        elif count <= easy_max_vowels and len(ln) <= easy_max_length:
            print("[Easy]: " + ln)
            print("[Medium]: " + ln)
            easy_file.write(line)
            medium_file.write(line)

        elif count <= medium_max_vowels and len(ln) <= medium_max_length:
            print("[Medium]: " + ln)
            medium_file.write(line)
            
    else:
        print("[Insane]: " + ln)
        insane_file.write(line)
