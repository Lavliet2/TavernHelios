export const getLocalISODate = (dateString: string): string => {
    const date = new Date(dateString);
    return new Date(date.getTime() - date.getTimezoneOffset() * 60000).toISOString();
};

  export default getLocalISODate;