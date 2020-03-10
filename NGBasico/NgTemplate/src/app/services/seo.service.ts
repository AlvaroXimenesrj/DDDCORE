
import { Injectable } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { BrowserModule } from "@angular/platform-browser/src/browser";

import { StringUtils } from "../utils/string.utils";


@Injectable()
export class SeoService {
    private titleService: Title;
    private metaService: Meta;

    public constructor(titleService: Title, metaService: Meta){
        this.titleService = titleService;
        this.metaService = metaService;
        this.setTitle('');
    }

    public setSeoData(seoModel: SeoModel){
        this.setTitle(seoModel.title);
        this.setMetaRobots(seoModel.robots)
        this.setMetaDescription(seoModel.description)
        this.setMetaKeywords(seoModel.keywords)
    }

    private setTitle(newTitle: string){
        if(StringUtils.isNullOrEmpty(newTitle)) {newTitle = "Defina um Título"}
        this.titleService.setTitle(newTitle + " - Eventos.IO")
    }

    private setMetaDescription(description: string) {
        if (StringUtils.isNullOrEmpty(description)) { description = "Aqui você encontra um evento técnico próximo de você" }
        this.metaService.removeTag('name="description"');
        this.metaService.addTag({ name: 'description', content: description });
    }

    private setMetaKeywords(keywords: string) {
        if (StringUtils.isNullOrEmpty(keywords)) { keywords = "eventos,workshops,encontros,congressos,comunidades,tecnologia" }
        this.metaService.removeTag('name="keywords"');
        this.metaService.addTag({ name: 'keywords', content: keywords });
    }

    private setMetaRobots(robots: string) {
        if (StringUtils.isNullOrEmpty(robots)) { robots = "all" }
        this.metaService.removeTag('name="robots"');
        this.metaService.addTag({ name: 'robots', content: robots });
    }
}

export class SeoModel{
    public title: string = '';
    public description: string = '';
    public robots: string = '';
    public keywords: string = '';
}