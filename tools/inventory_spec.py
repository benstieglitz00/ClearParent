import re, zipfile, hashlib, json
from pathlib import Path
from xml.etree import ElementTree as E
base=Path('ClearParent/ClearParent/docs')
p=base/'SourceSpecification.docx'
n={'w':'http://schemas.openxmlformats.org/wordprocessingml/2006/main'}
r=E.fromstring(zipfile.ZipFile(p).read('word/document.xml'))
paras=[''.join(t.text or '' for t in x.findall('.//w:t',n)).strip() for x in r.findall('.//w:p',n)]
titles={1:'User Management',2:'Parenting Journal Functional Specification',3:'Calendar Functional Specification',4:'Evidence Management',5:'Communication Management',6:'Parenting Time',7:'Expense Tracking',8:'Timeline',9:'Search',10:'AI Assistant',11:'Report Builder',12:'Dashboard',13:'Notifications',14:'Security',15:'Administration',16:'Data Import',17:'Analytics',18:'Mobile Application',19:'Parenting Plan Management'}
phases={1:1,2:2,3:3,4:2,5:4,6:3,7:3,8:4,9:4,10:5,11:4,12:5,13:4,14:1,15:5,16:5,17:5,18:5,19:3}
rows=[]; section='Preamble'; module='Preamble'; phase=0; decisions=[]; decision=False; counts={}; modules=[]
for i,text in enumerate(paras,1):
    for number,title in titles.items():
        if text.endswith(f'{number}. {title}'):
            counts[number]=counts.get(number,0)+1
            module=f'{number}. {title} (occurrence {counts[number]})'
            phase=phases[number]; section=module; decision=False
            modules.append((i,module,phase))
            break
    if re.match(r'^\d+\.\d+[^\d]',text):
        section=text; decision='Product Decisions' in text
    scope='TBD' if decision else 'Future scope' if 'Future' in section else 'MVP declaration' if 'MVP' in section else 'Acceptance criterion' if 'Acceptance Criteria' in section else 'Requires scope classification'
    if decision and text: decisions.append((i,module,text))
    if text:
        ids=re.findall(r'\b[A-Z]{2,}(?:-[A-Z]+)+-\d{3}\b',text)
        rows.append({'paragraph':i,'module':module,'section':section,'phase':phase,'scope':scope,'requirement_ids':ids,'text':text,'status':'Untriaged; see Traceability.md for validated partial coverage','implementation':None,'test':None})
(base/'RequirementInventory.json').write_text(json.dumps({'source_sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'note':'Lossless paragraph inventory including table cells. Paragraph locators preserve repeated Search section 9 and inconsistent Journal headings. 11. Report Builder occurs at the end of a preceding paragraph in the source. Scope labels describe containing section, not automatic approval or completion.','requirements':rows},indent=2,ensure_ascii=False),encoding='utf-8')
(base/'DecisionRegister.md').write_text('# Extracted product decisions\n\nSource paragraph locators; no policy is approved by extraction.\n\n'+'\n'.join(f'- P{i} [{m}]: {t}' for i,m,t in decisions),encoding='utf-8')
(base/'ModuleInventory.md').write_text('# Module inventory and dependency order\n\nOriginal numbering is preserved. Section 9 appears twice; neither occurrence is discarded. Module 11 begins inside the preceding paragraph. Child profiles and Contacts are referenced across modules but lack standalone numbered specifications; resolve their exact field ownership during Phase 2. Native mobile architecture remains TBD.\n\n| Source paragraph | Module | Planned phase | Dependencies |\n|---|---|---|---|\n'+'\n'.join(f'| P{i} | {m} | {phase} | '+('Identity, SQL schema, audit, authorization' if phase>1 else 'Repository/configuration; SQL validation')+' |' for i,m,phase in modules),encoding='utf-8')
print(f'Inventoried {len(paras)} paragraphs; {len(rows)} nonempty; {len(decisions)} decision paragraphs; {len(modules)} module occurrences.')
assert len(rows)==sum(bool(x) for x in paras)
assert {str(k)+'. ' for k in titles}.issubset({m.split(' ',1)[0]+' ' for _,m,_ in modules})
