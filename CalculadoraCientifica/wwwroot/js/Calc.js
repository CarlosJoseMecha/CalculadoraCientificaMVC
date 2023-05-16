(function () {
   "use strict";
   var buttons = document.querySelectorAll("button");
   var display = new Display();
   var calculator = new Calculator(display);
   var requestHandler = new RequestHandler();

   function Display() {
      this.display1 = document.getElementById("display1");
      this.display2 = document.getElementById("display2");
   }

   Display.prototype.updateDisplay = function () {
      this.display1.innerHTML = this.beautifyDisplay(calculator.memory);
      this.display2.innerHTML = this.beautifyDisplay(calculator.currentValue);
   };

   Display.prototype.resetDisplay = function () {
      this.display1.innerHTML = "";
      this.display2.innerHTML = "0";
   };

   Display.prototype.beautifyDisplay = function (expression) {
      expression = this.replaceSymbols(expression);
      expression = this.formatExpression(expression);
      return expression;
   };

   Display.prototype.replaceSymbols = function (expression) {
      expression = this.replaceReplacements(expression);
      expression = this.replaceLogs(expression);
      return expression;
   };

   Display.prototype.replaceReplacements = function (expression) {
      var replacements = {
         PI: "π",
         PHI: "φ",
         TAU: "τ",
         sqrt: "√",
         "\\^2": "²",
         "\\^3": "³",
      };
      for (var key in replacements) {
         expression = expression.replace(new RegExp(key, "g"), replacements[key]);
      }
      return expression;
   };

   Display.prototype.replaceLogs = function (expression) {
      var parts = expression.split("(");
      for (var i = 0; i < parts.length; i++) {
         if (parts[i].endsWith("log10")) {
            parts[i] = parts[i].slice(0, -5) + "log";
         } else if (parts[i].endsWith("log2")) {
            parts[i] = parts[i].slice(0, -4) + "log₂";
         } else if (parts[i].endsWith("log")) {
            parts[i] = parts[i].slice(0, -3) + "ln";
         }
      }
      return parts.join("(");
   };

   Display.prototype.formatExpression = function (expression) {
      return expression.replace(/([+\-÷*/])/g, function (match, offset, string) {
         if (string[offset - 1] === "e" && string[offset + 1] === "p") {
            return match;
         } else if (match === "/") {
            return " ÷ ";
         } else if (match === "*") {
            return " x ";
         } else if (
            match === "x" &&
            (offset === 0 || string[offset - 1] === " ") &&
            (offset === string.length - 1 || string[offset + 1] === " ")
         ) {
            return " x ";
         } else {
            return " " + match + " ";
         }
      });
   };

   function Calculator() {
      this.currentValue = "&nbsp;";
      this.memory = "&nbsp;";
      this.calculationPerformed = false;
      this.lastFunctionUsed = "";
      this.ans = "";
      this.openParenthesis = 0;
      this.closeParenthesis = 0;
   }

   Calculator.prototype.setFunctionStrategy = function (strategy) {
      this.functionStrategy = strategy;
   };

   Calculator.prototype.executeFunction = function (value) {
      this.functionStrategy.execute(this, value);
   };

   Calculator.prototype.initFunctionStrategies = function () {
      var strategies = ["cos", "sin", "tan", "sqrt", "log", "log10", "log2"];
      var functionStrategies = {
         rand: new RandStrategy(),
      };

      strategies.forEach(function (strategy) {
         functionStrategies[strategy] = new FunctionStrategy();
      });
      return functionStrategies;
   };

   Calculator.prototype.executeMathFunction = function (value) {
      var functionStrategies = this.initFunctionStrategies();

      if (functionStrategies.hasOwnProperty(value)) {
         this.setFunctionStrategy(functionStrategies[value]);
         this.executeFunction(value);
         if (
            ["cos", "sin", "tan", "sqrt", "log", "log10", "log2"].indexOf(value) !==
            -1
         ) {
            this.openParenthesisController();
         }
      } else {
         this.setFunctionStrategy(new FunctionStrategy());
         this.executeFunction(value);
      }
   };

   ///Memory
   Calculator.prototype.addNewValue = function (value) {
      if (this.currentValue === "&nbsp;") {
         this.currentValue = value;
         this.resetMemoryAndCalculationPerformed();
      } else if (this.calculationPerformed) {
         if (["+", "-", "*", "/"].includes(value)) {
            this.currentValue += value;
            this.setCalculationPerformed(false);
         } else {
            this.currentValue = value;
            this.resetMemoryAndCalculationPerformed();
         }
      } else {
         this.handleOperatorInput(value);
      }
   };

   Calculator.prototype.handleOperatorInput = function (value) {
      if (this.currentValue === "Syntax Error") {
         this.currentValue = value;
      } else {
         var lastChar = this.currentValue[this.currentValue.length - 1];
         if (
            ["+", "-", "*", "/"].includes(lastChar) &&
            ["+", "-", "*", "/"].includes(value)
         ) {
            if (value !== lastChar) {
               this.currentValue = this.currentValue.slice(0, -1) + value;
            }
         } else {
            this.currentValue += value;
         }
      }
   };

   Calculator.prototype.resetMemory = function () {
      this.memory = "&nbsp;";
   };

   Calculator.prototype.resetMemoryAndCalculationPerformed = function () {
      this.resetMemory();
      this.setCalculationPerformed(false);
   };

   Calculator.prototype.saveExpressionToMemory = function (expression) {
      this.memory = expression + " =";
   };

   Calculator.prototype.getAns = function () {
      if (this.ans !== "") {
         var lastChar = this.currentValue[this.currentValue.length - 1];
         if (["+", "-", "*", "/", "("].includes(lastChar)) {
            this.currentValue += this.ans;
         } else {
            this.currentValue = this.ans;
         }
      }
   };

   Calculator.prototype.resetAns = function () {
      this.ans = "&nbsp;";
   };

   ///Parenthesis management
   Calculator.prototype.decrementParenthesisCount = function (char) {
      if (char === "(") {
         this.openParenthesis--;
      } else if (char === ")") {
         this.closeParenthesis--;
      }
   };

   Calculator.prototype.resetParenthesis = function () {
      this.openParenthesis = 0;
      this.closeParenthesis = 0;
   };

   Calculator.prototype.openParenthesisController = function () {
      if (this.lastFunctionUsed !== "synErr") {
         var openParenthesisButton = document.querySelector(
            'button[data-value="("]'
         );
         openParenthesisButton.innerHTML =
            "( <sub class='openParenthesis'>" + this.openParenthesis + "</sub>";
         this.addNewValue("(");
         this.openParenthesis++;
      }
   };

   Calculator.prototype.closeParenthesisController = function () {
      if (this.lastFunctionUsed !== "synErr") {
         var closeParenthesisButton = document.querySelector(
            'button[data-value=")"]'
         );
         closeParenthesisButton.innerHTML =
            " <sub class='closeParenthesis'>" + this.closeParenthesis + "</sub> )";
         this.addNewValue(")");
         this.closeParenthesis++;
      }
   };

   Calculator.prototype.updateParenthesisButtons = function () {
      var openParenthesisButton = document.querySelector(
         'button[data-value="("]'
      );
      if (this.openParenthesis > 0) {
         openParenthesisButton.innerHTML =
            "( <sub class='openParenthesis'>" + this.openParenthesis + "</sub>";
      } else {
         openParenthesisButton.innerHTML = "(";
      }

      var closeParenthesisButton = document.querySelector(
         'button[data-value=")"]'
      );
      if (this.closeParenthesis > 0) {
         closeParenthesisButton.innerHTML =
            "<sub class='closeParenthesis'>" + this.closeParenthesis + "</sub> )";
      } else {
         closeParenthesisButton.innerHTML = ")";
      }
   };

   ///Operation management
   Calculator.prototype.checkIfCurrentValueEmpty = function () {
      if (this.currentValue === "") {
         this.currentValue = "&nbsp;";
      }
   };

   Calculator.prototype.resetCurrentValue = function () {
      this.currentValue = "&nbsp;";
   };

   Calculator.prototype.resetLastFunctionUsed = function () {
      this.lastFunctionUsed = "";
   };

   Calculator.prototype.setCalculationPerformed = function (value) {
      this.calculationPerformed = value;
   };

   Calculator.prototype.setLastFunctionUsed = function (value) {
      this.lastFunctionUsed = value;
   };

   Calculator.prototype.updateOperator = function (newOperator) {
      var lastChar = this.currentValue[this.currentValue.length - 1];
      if (["+", "-", "x", "÷"].includes(lastChar)) {
         if (newOperator === lastChar) {
            return;
         } else {
            this.currentValue = this.currentValue.slice(0, -1) + newOperator;
         }
      } else {
         this.currentValue += newOperator;
      }
   };

   ///General management
   Calculator.prototype.reset = function () {
      this.resetCurrentValue();
      this.resetMemory();
      this.resetLastFunctionUsed();
      this.resetAns();
      this.resetParenthesis();
   };

   Calculator.prototype.delete = function () {
      if (this.calculationPerformed) {
         this.resetMemory();
      } else {
         this.deleteLastCharacter();
      }
   };

   Calculator.prototype.deleteLastCharacter = function () {
      if (this.currentValue.length >= 1 && this.currentValue !== "&nbsp;") {
         var lastChar = this.currentValue[this.currentValue.length - 1];
         this.decrementParenthesisCount(lastChar);
         var constants = [
            "PI",
            "PHI",
            "TAU",
            "^2",
            "^3",
            "Syntax Error",
            "deg",
            "cos(",
            "sin(",
            "tan(",
            "log(",
            "log10(",
            "log2(",
            "sqrt(",
         ];
         var found = false;
         for (var i = 0; i < constants.length; i++) {
            var constant = constants[i];
            if (this.currentValue.endsWith(constant)) {
               this.currentValue = this.currentValue.slice(0, -constant.length);
               found = true;
               break;
            }
         }
         if (!found) {
            this.currentValue = this.currentValue.slice(0, -1);
         }
         this.checkIfCurrentValueEmpty();
      } else {
         this.resetCurrentValue();
      }
   };

   Calculator.prototype.solve = function () {
      if (this.currentValue !== "0") {
         this.saveExpressionToMemory(this.currentValue);
         try {
            this.currentValue = math
               .evaluate(this.currentValue.toLocaleLowerCase())
               .toString();
            this.ans = this.currentValue;
            this.setCalculationPerformed(true);
            this.resetParenthesis();
            requestHandler.sendToController(this.memory, this.currentValue);
         } catch (error) {
            this.currentValue = "Syntax Error";
            requestHandler.sendToController(this.memory, this.currentValue);
            this.resetMemory();
            this.resetParenthesis();
            this.setLastFunctionUsed("synErr");

         }
      }
   };

   function FunctionStrategy() { }

   FunctionStrategy.prototype.execute = function (calculator, value) {
      if (calculator.lastFunctionUsed !== "synErr") {
         calculator.addNewValue(value);
         calculator.setLastFunctionUsed(value);
      } else {
         calculator.resetCurrentValue();
         calculator.resetLastFunctionUsed();
         calculator.addNewValue(value);
         calculator.setLastFunctionUsed(value);
      }
   };

   function RandStrategy() { }
   RandStrategy.prototype = Object.create(FunctionStrategy.prototype);

   RandStrategy.prototype.execute = function (calculator) {
      if (
         calculator.lastFunctionUsed !== "rand" &&
         calculator.lastFunctionUsed !== "synErr"
      ) {
         calculator.addNewValue(Math.random().toString());
         calculator.setLastFunctionUsed("rand");
      } else {
         calculator.resetCurrentValue();
         calculator.addNewValue(Math.random().toString());
      }
   };

   function RequestHandler() { };

   RequestHandler.prototype.formatDate = function (date) {
      var year = date.getFullYear();
      var month = ('0' + (date.getMonth() + 1)).slice(-2);
      var day = ('0' + date.getDate()).slice(-2);
      var hour = ('0' + date.getHours()).slice(-2);
      var minute = ('0' + date.getMinutes()).slice(-2);
      var second = ('0' + date.getSeconds()).slice(-2);
      return year + '-' + month + '-' + day + ' | ' + hour + ':' + minute + ':' + second;
   }

   RequestHandler.prototype.sendToController = function (expression, result) {
      var formattedDate = this.formatDate(new Date());

      if (expression === "&nbsp;") {
         expression = "null";
      }

      $.ajax({
         url: '/Historial/Create',
         type: 'POST',
         contentType: 'application/json',
         data: JSON.stringify({ Expresion: expression, Resultado: result, FechaHora: formattedDate }),
         success: function (response) {
            console.log("OK")
         }
      });
   }


   var specialActionsController = {
      C: function () {
         calculator.delete();
      },
      AC: function () {
         calculator.reset();
      },
      CE: function () {
         calculator.resetCurrentValue();
      },
      "=": function () {
         calculator.solve();
      },
      "(": function () {
         calculator.openParenthesisController();
      },
      ")": function () {
         calculator.closeParenthesisController();
      },
      ans: function () {
         calculator.getAns();
      },
   };

   function handleButtonClick(event) {
      var target = event.target;
      // Busca el elemento button más cercano en la jerarquía del DOM - Fix para el icono de la tecla borrar.
      while (target && target.tagName !== "BUTTON") {
         target = target.parentElement;
      }

      if (!target) return;
      var value = target.getAttribute("data-value");
      if (specialActionsController.hasOwnProperty(value)) {
         specialActionsController[value]();
      } else if (target.classList.contains("function")) {
         calculator.executeMathFunction(value);
      } else {
         calculator.addNewValue(value);
      }
      display.updateDisplay();
      calculator.updateParenthesisButtons();
   }

   buttons.forEach(function (button) {
      button.addEventListener("click", handleButtonClick);
   });
})();
