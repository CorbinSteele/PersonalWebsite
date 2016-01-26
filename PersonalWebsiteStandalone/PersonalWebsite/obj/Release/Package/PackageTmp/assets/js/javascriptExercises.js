
function getInput() {
    var $input = $("#jsExercisesTabContent > .active .jsInput");
    return ($input.length == 1) ? $input.val().trim() :
        $input.map(function () { return $(this).val().trim(); }).get();
}

function setOutput(value) {
    var $output = $("#jsExercisesTabContent > .active .jsOutput");
    var outputValue = asStringArray(value);
    var outputLength = outputValue.length / $output.length;
    $output.each(function (index, output) {
        $(output).val(outputValue
            .slice(outputLength * index, outputLength * (index + 1))
            .join("\n"));
    });
    $output.collapse('show');
}

function asStringArray(obj) {
    var str = [];
    for (var p in obj)
        if (obj.hasOwnProperty(p))
            str.push(p + ": " + obj[p]);
    return str;
}

function analyzeNumbers(numbers) {
    if (!numbers.length)
        return { Error: "There were no numbers." };
    var least = greatest = sum = product = numbers[0];
    for (i = 1; i < numbers.length; i++) {
        var number = numbers[i];
        if (number < least)
            least = number;
        if (number > greatest)
            greatest = number;
        sum += number;
        product *= number;
    }
    var mean = sum / numbers.length;
    return {
        Least: least, Greatest: greatest,
        Sum: sum, Product: product, Mean: mean
    };
}

function submitNumbersToAnalyze() {
    var numbers = getInput().split(/\s+/).map(Number)
                  .filter(function (number, _, __) {
                      return number === number;
                  });
    setOutput(analyzeNumbers(getInput() ? numbers : []));
}

function factorial(number) {
    var factorial = 1;
    for (i = number; i > 0; i--)
        factorial *= i;
    return factorial;
}

function submitNumberToFactorialize() {
    var number = Number(getInput());
    if (number % 1 && number != 0)
        setOutput({
            Error: "It's too hard to calculate that!"
        });
    else if (getInput() && number >= 0)
        setOutput({ Factorial: factorial(number) });
    else
        setOutput({ Fact: "That has no factorial." });
}

function fizzBuzz(number1, number2) {
    var output = [];
    for (i = 1; i <= 100; i++)
        output[i] = (i % number1 ? "" : "Fizz")
            + (i % number2 ? "" : "Buzz") || i;
    return output;
}

function submitNumbersToFizzBuzz() {
    var numbers = getInput().map(function (n) {
        return Number(n) ? Number(n) : Infinity;
    });
    setOutput(fizzBuzz(numbers[0], numbers[1]));
}

function palindrome(str) {
    for (i = 0; i < str.length / 2;)
        if (str[i].toLowerCase() != str[str.length-++i].toLowerCase())
            return false;
    return true;
}

function submitStringToPalindrome() {
    var str = getInput().split("");
    var strLength = str.length;
    if (!strLength)
        setOutput({ Error: "Nothing was entered." })
    else if (palindrome(str))
        setOutput({
            "That is a Palindrome" :
                "\n" + str.splice(0, strLength / 2).join("") +
                " <" + (!(strLength % 2) ? "-" : str.shift()) + "> "
                + str.join("")
        });
    else
        setOutput({
            "That is not a Palindrome" : "\n" + str.reverse().join("")
        });
}

/*
var numbersString = prompt("Enter some numbers separated by spaces.");
var numbers = numbersString.split(" ").map(Number);
var least = numbers[0];
var greatest = numbers[0];
var sum = numbers[0];
var product = numbers[0];
for (i = 1; i < numbers.length; i++) {
	var number = numbers[i];
	if (number < least) {
		least = number;
	}
	if (number > greatest) {
		greatest = number;
	}
	sum += number;
	product *= number;
}
var mean = sum / numbers.length;
window.alert("Least: " + least + "; Greatest: " + greatest
	+ "; Mean: " + mean + "; Sum: " + sum + "; Product: "
	+ product + ".");


var number = prompt("Enter some number to factorialize.");
var factorial = 1;
for (i = number; i > 0; i--) {
	factorial *= i;
}
window.alert("The factorial of " + number + " is " + factorial + ".");


var number1 = prompt("Enter the first of two numbers.");
var number2 = prompt("Enter the second of two numbers.");
var output = [];
for (i = 1; i <= 100; i++) {
	output[i - 1] = "";
	if (!(i % number1)) {
		output[i - 1] += "Fizz";
	}
	if (!(i % number2)) {
		output[i - 1] += "Buzz"
	}
	if (!output[i - 1]) {
		output[i - 1] = i;
	}
}
window.alert(function() {
	var outputString = "The result for " + number1 + " and " + number2 + " is:\n";
	for (i = 0; i < output.length; i++) {
		outputString += output[i] + "\n";
	}
	return outputString;
}());


var pal = prompt("Enter a str to see if it's a palindrome.");
for (i = 0; i < pal.length / 2; i++) {
	if (pal[i] != pal[pal.length - i - 1]) {
		var notPal = true;
		break;
	}
}
window.alert("The str " + pal + " is " + ((notPal) ? "not " : "") + "a palindrome.");
*/