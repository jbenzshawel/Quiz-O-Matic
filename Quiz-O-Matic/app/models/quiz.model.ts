export class Quiz {
    
    public id: string;
    
    public name: string;

    public description: string;

    public attributes: any;

    public images: any;

    public type: string;

    public typeId: string;

    public created: Date; 

    public updated: Date;

    constructor(id: string, name:string, description: string, attributes: any, images: any, type: string, typeId: string, created: Date, updated: Date = null)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.attributes = attributes;
        this.images = images;
        this.type = type;
        this.typeId = typeId;
        this.created = created;
        this.updated = updated;
    }
}
    