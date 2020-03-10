export class StringUtils {
    public static isNullOrEmpty(val: string) : boolean {
        if (val === undefined || val === null || val.trim() === '') {
            return true;
        }
        return false;
    };
}

// Não considera o tipo
// '1' == 1 = true
// '1' != 1 = false

// Considera o tipo
// '1' === 1 = false
// '1' !== 1 = true
