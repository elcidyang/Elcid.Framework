var idCardNoUtil = {
    /*ʡ,ֱϽ�д����*/
    provinceAndCitys: {
        11: "����", 12: "���", 13: "�ӱ�", 14: "ɽ��", 15: "���ɹ�", 21: "����", 22: "����", 23: "������",
        31: "�Ϻ�", 32: "����", 33: "�㽭", 34: "����", 35: "����", 36: "����", 37: "ɽ��", 41: "����", 42: "����", 43: "����", 44: "�㶫",
        45: "����", 46: "����", 50: "����", 51: "�Ĵ�", 52: "����", 53: "����", 54: "����", 61: "����", 62: "����", 63: "�ຣ", 64: "����",
        65: "�½�", 71: "̨��", 81: "���", 82: "����", 91: "����"
    },
    /*ÿλ��Ȩ����*/
    powers: ["7", "9", "10", "5", "8", "4", "2", "1", "6", "3", "7", "9", "10", "5", "8", "4", "2"],

    /*��18λУ����*/
    parityBit: ["1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2"],

    /*�Ա�*/
    genders: { male: "��", female: "Ů" },

    /*У���ַ��*/
    checkAddressCode: function (addressCode) {
        var check = /^[1-9]\d{5}$/.test(addressCode);
        if (!check) return false;
        if (idCardNoUtil.provinceAndCitys[parseInt(addressCode.substring(0, 2))]) {
            return true;
        } else {
            return false;
        }
    },

    /*У��������*/
    checkBirthDayCode: function (birDayCode) {
        var check = /^[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))$/.test(birDayCode);
        if (!check) return false;
        var yyyy = parseInt(birDayCode.substring(0, 4), 10);
        var mm = parseInt(birDayCode.substring(4, 6), 10);
        var dd = parseInt(birDayCode.substring(6), 10);
        var xdata = new Date(yyyy, mm - 1, dd);
        if (xdata > new Date()) {
            return false;//���ղ��ܴ��ڵ�ǰ����
        } else if ((xdata.getFullYear() == yyyy) && (xdata.getMonth() == mm - 1) && (xdata.getDate() == dd)) {
            return true;
        } else {
            return false;
        }
    },

    /*����У����*/
    getParityBit: function (idCardNo) {
        var id17 = idCardNo.substring(0, 17);
        /*��Ȩ */
        var power = 0;
        for (var i = 0; i < 17; i++) {
            power += parseInt(id17.charAt(i), 10) * parseInt(idCardNoUtil.powers[i]);
        }
        /*ȡģ*/
        var mod = power % 11;
        return idCardNoUtil.parityBit[mod];
    },

    /*��֤У����*/
    checkParityBit: function (idCardNo) {
        var parityBit = idCardNo.charAt(17).toUpperCase();
        if (idCardNoUtil.getParityBit(idCardNo) == parityBit) {
            return true;
        } else {
            return false;
        }
    },

    /*У��15λ��18λ�����֤����*/
    checkIdCardNo: function (idCardNo) {
        //15λ��18λ���֤����Ļ���У��
        var check = /^\d{15}|(\d{17}(\d|x|X))$/.test(idCardNo);
        if (!check) return false;
        //�жϳ���Ϊ15λ��18λ  
        if (idCardNo.length == 15) {
            return idCardNoUtil.check15IdCardNo(idCardNo);
        } else if (idCardNo.length == 18) {
            return idCardNoUtil.check18IdCardNo(idCardNo);
        } else {
            return false;
        }
    },

    //У��15λ�����֤����
    check15IdCardNo: function (idCardNo) {
        //15λ���֤����Ļ���У��
        var check = /^[1-9]\d{7}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}$/.test(idCardNo);
        if (!check) return false;
        //У���ַ��
        var addressCode = idCardNo.substring(0, 6);
        check = idCardNoUtil.checkAddressCode(addressCode);
        if (!check) return false;
        var birDayCode = '19' + idCardNo.substring(6, 12);
        //У��������
        return idCardNoUtil.checkBirthDayCode(birDayCode);
    },

    //У��18λ�����֤����
    check18IdCardNo: function (idCardNo) {
        //18λ���֤����Ļ�����ʽУ��
        var check = /^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}(\d|x|X)$/.test(idCardNo);
        if (!check) return false;
        //У���ַ��
        var addressCode = idCardNo.substring(0, 6);
        check = idCardNoUtil.checkAddressCode(addressCode);
        if (!check) return false;
        //У��������
        var birDayCode = idCardNo.substring(6, 14);
        check = idCardNoUtil.checkBirthDayCode(birDayCode);
        if (!check) return false;
        //��֤У����   
        return idCardNoUtil.checkParityBit(idCardNo);
    },

    formateDateCN: function (day) {
        var yyyy = day.substring(0, 4);
        var mm = day.substring(4, 6);
        var dd = day.substring(6);
        return yyyy + '-' + mm + '-' + dd;
    },

    //��ȡ��Ϣ
    getIdCardInfo: function (idCardNo) {
        var idCardInfo = {
            gender: "",   //�Ա�
            birthday: "" // ��������(yyyy-mm-dd)
        };
        if (idCardNo.length == 15) {
            var aday = '19' + idCardNo.substring(6, 12);
            idCardInfo.birthday = idCardNoUtil.formateDateCN(aday);
            if (parseInt(idCardNo.charAt(14)) % 2 == 0) {
                idCardInfo.gender = idCardNoUtil.genders.female;
            } else {
                idCardInfo.gender = idCardNoUtil.genders.male;
            }
        } else if (idCardNo.length == 18) {
            var aday = idCardNo.substring(6, 14);
            idCardInfo.birthday = idCardNoUtil.formateDateCN(aday);
            if (parseInt(idCardNo.charAt(16)) % 2 == 0) {
                idCardInfo.gender = idCardNoUtil.genders.female;
            } else {
                idCardInfo.gender = idCardNoUtil.genders.male;
            }

        }
        return idCardInfo;
    },

    /*18λת15λ*/
    getId15: function (idCardNo) {
        if (idCardNo.length == 15) {
            return idCardNo;
        } else if (idCardNo.length == 18) {
            return idCardNo.substring(0, 6) + idCardNo.substring(8, 17);
        } else {
            return null;
        }
    },

    /*15λת18λ*/
    getId18: function (idCardNo) {
        if (idCardNo.length == 15) {
            var id17 = idCardNo.substring(0, 6) + '19' + idCardNo.substring(6);
            var parityBit = idCardNoUtil.getParityBit(id17);
            return id17 + parityBit;
        } else if (idCardNo.length == 18) {
            return idCardNo;
        } else {
            return null;
        }
    }
};

var dateUtil = {
    /**
     * ��ȡ����
     * @param {any} strDate ����
     * @param {any} days ����
     */
    addDate: function (strDate, days) {
        var date = new Date(strDate);
        date = date.valueOf();
        date = date + days * 24 * 60 * 60 * 1000;
        date = new Date(date);
        return date;
    },

    /**
 * ����תΪ�ַ���
 * @param {��ʽ} fmt 
 * @returns {�ַ���} 
 */
    dateFormat: function (date, fmt) {
        var o = {
            "M+": date.getMonth() + 1, //�·� 
            "d+": date.getDate(), //�� 
            "h+": date.getHours(), //Сʱ 
            "m+": date.getMinutes(), //�� 
            "s+": date.getSeconds(), //�� 
            "q+": Math.floor((date.getMonth() + 3) / 3), //���� 
            "S": date.getMilliseconds() //���� 
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
};