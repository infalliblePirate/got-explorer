/* eslint-disable @typescript-eslint/no-explicit-any */

const ErrorHandle = (errorlist: any) => {
    let line = "";
    errorlist.forEach((item: any) => {
        line += item.errorMessage + " ";
    })
    return line;
}

export default ErrorHandle;