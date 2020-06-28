$.validator.methods.date = function (value, element) {
    return this.optional(element) || $.datepicker.parseDate('dd.mm.yyyy', value);
}